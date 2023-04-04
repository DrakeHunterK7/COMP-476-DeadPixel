using UnityEngine;

namespace AI
{
    public struct SteeringOutput
    {
        public Vector3 _velocity;
        public Quaternion _rotation;


        public SteeringOutput(Vector2 velocity, Quaternion rotation)
        {
            _velocity = velocity;
            _rotation = rotation;
        }
    }

}
