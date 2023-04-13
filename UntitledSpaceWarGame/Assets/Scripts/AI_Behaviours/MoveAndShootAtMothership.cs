using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using BehaviourTree;

public class MoveAndShootAtMothership : Node
{
    private ShipAIBT owner;
    private Pursue chaseMovement;
    private AIAgent ownerAgent;

    private float shootTimer = 0.1f;

    public MoveAndShootAtMothership(ShipAIBT ownerShip)
    {
        owner = ownerShip;
        chaseMovement = owner.GetComponent<Pursue>();
        ownerAgent = owner.GetComponent<AIAgent>();
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject) owner.GetRootData("EnemyMothership");

        if (target != null)
        {
            chaseMovement.SetTarget(target.transform);
            ownerAgent.SetActiveMovement(chaseMovement);

            if (Vector3.Dot(owner.transform.forward.normalized,
                    (target.transform.position - owner.transform.position).normalized) > 0.9f)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer < 0)
                {
                    owner.Shoot();
                    shootTimer = 0.1f;
                }
            }
            else
            {
                shootTimer = 0.1f;
            }
        }


        state = NodeState.RUNNING;
        return state;
    }
}
