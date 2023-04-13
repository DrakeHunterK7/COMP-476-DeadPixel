using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class CheckForEnemyMothership : Node
    {
        private ShipAIBT ownerShip;
        private float radius = 2000;
        private float angle = 120;
        private GameObject enemyMothership;
        public CheckForEnemyMothership(ShipAIBT owner)
        {
            ownerShip = owner;
        }

        public override NodeState Evaluate()
        {
            if(enemyMothership == null)
                enemyMothership = CheckFOV();

            if (enemyMothership != null)
            {
                ownerShip.SetRootData("EnemyMothership", enemyMothership);
                if(Vector3.Distance(enemyMothership.transform.position, ownerShip.transform.position) < 1500
                   && Vector3.Dot(ownerShip.transform.forward, (enemyMothership.transform.position - ownerShip.transform.position)) > 0.65f)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }


                if (Vector3.Distance(enemyMothership.transform.position, ownerShip.transform.position) < 500)
                {
                    enemyMothership = null;
                    ownerShip.ClearRootData("EnemyMothership");
                }
                
            }
            else
            {
                ownerShip.ClearRootData("EnemyMothership");
            }
            
            state = NodeState.FAILURE;
            return state;
        }
        
        public GameObject CheckFOV()
        {
            Collider[] colliders = new Collider[20];
            var hitCount = Physics.OverlapSphereNonAlloc(ownerShip.transform.position, radius, colliders, LayerMask.GetMask("Obstacles"));

            GameObject enemyMothership = null;

            for (var i = 0; i < hitCount; i++)
            {
                var vectorToTarget = (colliders[i].transform.position - ownerShip.transform.position);
                var directionToTarget = vectorToTarget.normalized;

                var angleToTarget = Vector3.Angle(directionToTarget, ownerShip.transform.forward);

                if (angleToTarget >= -angle && angleToTarget <= angle || vectorToTarget.magnitude > 500)
                {
                    var ray1 = new Ray(ownerShip.transform.position, directionToTarget);
                    RaycastHit hitData1;
                
                    if (Physics.Raycast(ray1, out hitData1, vectorToTarget.magnitude))
                    {
                        var agent = hitData1.collider.gameObject;

                        if (agent.CompareTag("Mothership"))
                        {
                            var script = agent.GetComponent<Mothership>();

                            if (script != null && script._team != ownerShip.shipInformation.GetTeam())
                            {
                                return agent;
                            }
                        }
                    }
                }
            }
            
            return enemyMothership;
        }
    }
}