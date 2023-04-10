using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using BehaviourTree;

public class  Task_Chase : Node
{
    private ShipAIBT owner;
    private Pursue chaseMovement;
    private AIAgent ownerAgent;

    public Task_Chase(ShipAIBT ownerShip)
    {
        owner = ownerShip;
        chaseMovement = owner.GetComponent<Pursue>();
        ownerAgent = owner.GetComponent<AIAgent>();
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject) owner.GetRootData("Target"); // target refers to the ship that you're chasing

        if (target != null)
        {
            //WRITE CHASE CODE HERE
            
            chaseMovement.SetTarget(target.transform);
            ownerAgent.SetActiveMovement(chaseMovement);
            
        }


        state = NodeState.RUNNING;
        return state;
    }
}
