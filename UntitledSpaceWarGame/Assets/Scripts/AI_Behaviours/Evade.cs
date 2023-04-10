using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using BehaviourTree;
using UnityEditor.Rendering;

public class Task_Evade : Node
{
    private ShipAIBT owner;
    private Evade evadeMovement;
    private AIAgent ownerAgent;
    private float timeCount = 0f;

    public Task_Evade(ShipAIBT ownerShip)
    {
        owner = ownerShip;
        evadeMovement = owner.GetComponent<Evade>();
        ownerAgent = owner.GetComponent<AIAgent>();
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject) owner.GetRootData("Target"); // target refers to the ship that is chasing you
        evadeMovement.SetTarget(target.transform); 
        
        if (target != null)
        {
            //WRITE EVADE CODE HERE
            
            evadeMovement.SetTarget(target.transform);
            ownerAgent.SetActiveMovement(evadeMovement);
        }


        state = NodeState.RUNNING;
        return state;
    }
}
