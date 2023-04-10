using AI;
using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class CheckOutOfBounds : Node
    {
        private ShipAIBT ownerShip;

        public CheckOutOfBounds(ShipAIBT owner)
        {
            ownerShip = owner;
        }

        public override NodeState Evaluate()
        {
            var ownerPosition = ownerShip.transform.position;

            if (ownerPosition.x < -10000 || ownerPosition.x > 10000
                || ownerPosition.y < -10000 || ownerPosition.y > 10000
                || ownerPosition.z < -10000 || ownerPosition.z > 10000)
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}