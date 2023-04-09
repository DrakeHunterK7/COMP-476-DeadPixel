using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class CheckDistanceFromThreat : Node
    {
        private ShipAIBT ownerShip;

        public CheckDistanceFromThreat(ShipAIBT owner)
        {
            ownerShip = owner;
        }

        public override NodeState Evaluate()
        {
            GameObject target = (GameObject) ownerShip.GetRootData("Target");

            if (target != null)
            {
                if(Vector3.Distance(target.transform.position, ownerShip.transform.position) < 2500)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }
            
            state = NodeState.FAILURE;
            return state;
        }
    }
}