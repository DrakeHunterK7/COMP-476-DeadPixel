using UnityEngine;
using System.Collections.Generic;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        public float _maxForwardSpeed = 15.0f;
        public float _maxAngularRotationSpeed;

        [HideInInspector]
        private float _activeForwardSpeed;
        private float _activeAngularRotation;

        private float _forwardAcceleration = 2.5f;
        private float _angularAcceleration = 3.0f;

        // 1 for forward
        // 0 for stationary
        // -1 for backwards
        private int _applyForwardSpeed = 1;

        //[HideInInspector]
        //public Vector3 position;
        [HideInInspector]
        public Vector3 forward;

        public Vector3 _velocity;

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

            _velocity = Vector3.zero;

            _activeMovements = new List<AIMovement>();
            _activeForwardSpeed = 15.0f;

            //position = transform.position;
            _activeMovements.Add(gameObject.GetComponent<Pursue>());

    }

        private void Update()
        {
            // Draws vertex of movement
            //Debug.DrawRay(transform.position, _velocity, Color.red);

            steering = GetAcceleration();

            // sets the active forward speed
            //_activeForwardSpeed = Mathf.Lerp(_activeForwardSpeed, _applyForwardSpeed * _maxForwardSpeed, _forwardAcceleration * Time.deltaTime);


            _velocity = steering._velocity;

            //Debug.Log(_velocity);

            // Forward Movement
            transform.position += _velocity * Time.deltaTime;

            // Orientate the ai to where we want them to go to
            transform.rotation = Quaternion.Slerp(transform.rotation, steering._rotation, _maxAngularRotationSpeed *  Time.deltaTime);
        }

        // Calculates the blend of movements that act on the agent
        public SteeringOutput GetAcceleration()
        {
            Vector3 _velocityBlend = Vector3.zero;
            Quaternion _rotationBlend = Quaternion.identity;

            // TESTING
            _activeMovements.Add(gameObject.GetComponent<ObstacleAvoidance>());


            // Blend the movements together by weight
            foreach (AIMovement movement in _activeMovements)
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

                    if(q != Quaternion.identity && gameObject.GetComponent<ObstacleAvoidance>()._avoidingCollision == false) // Blends Rotation components together
                    {
                        _rotationBlend *= q;
                    }
                    else if(gameObject.GetComponent<ObstacleAvoidance>()._avoidingCollision == true && movement == gameObject.GetComponent<ObstacleAvoidance>())
                    {
                        _rotationBlend *= q;
                    }
                    
                }
            }

            SteeringOutput blend = new SteeringOutput(_velocityBlend, _rotationBlend);

            // Reset each frame
            _activeMovements = new List<AIMovement>();

            return blend;
        }

        // used by behavior scripts to apply some movements based on states
        public void SetActiveMovement(AIMovement movement)
        {
            _activeMovements.Add(movement);
        }


    }
}