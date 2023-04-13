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
            
            Debug.Log(target);

            if (target != null)
            {
                if(Vector3.Distance(target.transform.position, ownerShip.transform.position) < 300
                   && Vector3.Dot(target.transform.forward, (ownerShip.transform.position - target.transform.position)) > 0.65f)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }

                if (Vector3.Distance(target.transform.position, ownerShip.transform.position) > 1250)
                {
                    if(target.GetComponent<ShipAIBT>() != null)
                        target.GetComponent<ShipAIBT>().ClearRootData("Target");
                    ownerShip.ClearRootData("Target");
                }
            }
            else
            {
                ownerShip.ClearRootData("Target");
            }
            
            

            state = NodeState.FAILURE;
            return state;
        }
    }
}