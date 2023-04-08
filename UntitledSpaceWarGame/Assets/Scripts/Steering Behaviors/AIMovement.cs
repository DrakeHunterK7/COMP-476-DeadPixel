using UnityEngine;

namespace AI
{
    public abstract class AIMovement : MonoBehaviour
    {
        public bool debug;
        public float weight;

        public virtual SteeringOutput GetMovement(AIAgent agent)
        {
            return new SteeringOutput();
        }

    }
}
