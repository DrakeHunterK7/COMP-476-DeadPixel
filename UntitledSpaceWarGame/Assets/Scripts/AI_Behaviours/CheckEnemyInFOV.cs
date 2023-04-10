using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class CheckEnemyInFOV : Node
    {
        private ShipAIBT ownerShip;
        private float radius = 2000;
        private float angle = 180;

        public CheckEnemyInFOV(ShipAIBT owner)
        {
            ownerShip = owner;
        }

        public override NodeState Evaluate()
        {
            GameObject target = CheckFOV();

            // if (target != null)
            // {
            //     state = NodeState.SUCCESS;
            //     return state;
            // }

            state = NodeState.FAILURE;
            return state;
        }
        
        public GameObject CheckFOV()
        {
            Collider[] colliders = new Collider[20];
            var hitCount = Physics.OverlapSphereNonAlloc(ownerShip.transform.position, radius, colliders, LayerMask.GetMask("Ships"));

            GameObject target = null;

            for (var i = 0; i < hitCount; i++)
            {
                Debug.Log("A ship is near!");
                Debug.Log(colliders[i].gameObject.name);
                var vectorToTarget = (colliders[i].transform.position - ownerShip.transform.position);
                var directionToTarget = vectorToTarget.normalized;

                var angleToTarget = Vector3.Angle(directionToTarget, ownerShip.transform.forward);

                if ( angleToTarget >= -angle && angleToTarget <= angle)
                {
                    var ray1 = new Ray(ownerShip.transform.position, directionToTarget);
                    RaycastHit hitData1;
                
                    if (Physics.Raycast(ray1, out hitData1, vectorToTarget.magnitude))
                    {
                        var agent = hitData1.collider.gameObject;

                        var agentScript = agent.GetComponentInParent<ShipAIBT>();

                        if (agentScript != null)
                        {
                            if (agentScript.shipInformation._team != ownerShip.shipInformation._team)
                            {
                                target = agentScript.gameObject;
                                ownerShip.SetRootData("Target", target);
                                break;
                            }
                        }
                        else
                        {
                            var playerScript = agent.GetComponentInParent<ShipController>();
                            
                            if (playerScript.shipInformation._team != ownerShip.shipInformation._team)
                            {
                                Debug.Log("Player detected!");
                                target = playerScript.gameObject;
                                ownerShip.SetRootData("Target", target);
                                break;
                            }
                            
                        }

                    }
                }
            }

            return target;
        }
    }
}