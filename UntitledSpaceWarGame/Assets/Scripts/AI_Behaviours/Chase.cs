using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Task_Chase : Node
{
    private ShipAIBT owner;
    private ShipAIBT BT;

    public Task_Chase(ShipAIBT ownerShip)
    {
        owner = ownerShip;
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject) owner.GetRootData("Target"); // target refers to the ship that you're chasing

        if (target != null)
        {
            //WRITE CHASE CODE HERE
        }


        state = NodeState.RUNNING;
        return state;
    }
}
