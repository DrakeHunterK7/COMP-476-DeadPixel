using UnityEngine;
using System.Collections.Generic;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        public float _maxForwardSpeed;
        public float _maxAngularRotationSpeed;

        private float _activeForwardSpeed;
        private float _activeAngularRotation;

        private float _forwardAcceleration = 2.5f;
        private float _angularAcceleration = 3.0f;

        // 1 for forward
        // 0 for stationary
        // -1 for backwards
        private int _applyForwardSpeed = 1;

        [HideInInspector]
        public Vector3 position;
        [HideInInspector]
        public Vector3 forward;

        public Vector3 Velocity;

        [HideInInspector]
        Vector3 acceleration;

        [SerializeField]
        public bool freezeMovement;

        [HideInInspector]
        public List<AIMovement> _activeMovements;

        private SteeringOutput steering;

        public bool debugSteering;



        private void Awake()
        {

            Velocity = Vector3.zero;

            _activeMovements = new List<AIMovement>();


            position = transform.position;

    }

        private void Update()
        {
            // Draws vertex of movement
            Debug.DrawRay(transform.position, Velocity, Color.red);

            steering = GetAcceleration();

            // sets the active forward speed
            _activeForwardSpeed = Mathf.Lerp(_activeForwardSpeed, _applyForwardSpeed * _maxForwardSpeed, _forwardAcceleration * Time.deltaTime);

            transform.position += transform.forward * _activeForwardSpeed * Time.deltaTime;

            float speed = Velocity.magnitude;
            Vector3 dir;

            // This check needs to happen cause v/0 is invalid
            if (speed != 0)
            {
                dir = Velocity / speed;
            }
            else
            {
                dir = Vector3.zero;
            }

            // Clamp speed and apply velocity
            speed = Mathf.Clamp(speed, 0, _maxForwardSpeed);
            Velocity = dir * speed;

            // Apply transformation on ai position
            if(Velocity != Vector3.zero)
            {
                //Quaternion rotationToApply = Quaternion.RotateTowards(transform.rotation, Velocity.normalized, _maxAngularRotationSpeed * Time.deltaTime);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToApply, 720 * Time.deltaTime);

                forward = dir;
            }
        }

        // Calculates the blend of movements that act on the agent
        public SteeringOutput GetAcceleration()
        {
            Vector3 _velocityBlend = Vector3.zero;
            Quaternion _rotationBlend = Quaternion.identity;

            
            // Blend the movements together by weight
            foreach(AIMovement movement in _activeMovements)
            {
                if(movement != null)
                {
                    SteeringOutput s = movement.GetMovement(this);

                    Vector3 v = s._velocity;
                    Quaternion q = s._rotation;

                    if(v != Vector3.zero) // Blends the velocity components together
                    {
                        v = v * _maxForwardSpeed;// - Velocity;
                        _velocityBlend += Vector3.ClampMagnitude(v, _maxForwardSpeed) * movement.weight; // add the movement to the kinematic blend with its weight
                    }

                    if(q != Quaternion.identity) // Blends Rotation components together
                    {
                        _rotationBlend *= q;
                    }
                    
                }
            }

            SteeringOutput blend = new SteeringOutput(_velocityBlend, _rotationBlend);

            // Reset each frame
            //_activeMovements = new List<AIMovement>();

            return blend;
        }

        // used by behavior scripts to apply some movements based on states
        public void SetActiveMovement(AIMovement movement)
        {
            _activeMovements.Add(movement);
        }


    }
}