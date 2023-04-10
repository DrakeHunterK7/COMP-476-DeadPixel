using AI;
using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class GoBackInBounds : Node
    {
        private ShipAIBT ownerShip;
        private Seek seekMovement;
        private AIAgent ownerAgent;

        public GoBackInBounds(ShipAIBT owner)
        {
            ownerShip = owner;
            seekMovement = owner.GetComponent<Seek>();
            ownerAgent = owner.GetComponent<AIAgent>();
        }

        public override NodeState Evaluate()
        {
            GameObject mothership = (GameObject) ownerShip.GetRootData("Mothership");

            if (mothership != null)
            {
                seekMovement.SetTargetPosition(mothership.transform.position);
            }
            else
            {
                seekMovement.SetTargetPosition(Vector3.zero);
            }
            
            ownerAgent.SetActiveMovement(seekMovement);

            state = NodeState.RUNNING;
            return state;
        }
    }
}