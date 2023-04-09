using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Evade : Node
{
    private ShipAIBT owner;
    private ShipAIBT BT;

    public Evade(ShipAIBT ownerShip)
    {
        owner = ownerShip;
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject) owner.GetRootData("Target"); // target refers to the ship that is chasing you

        if (target != null)
        {
            //WRITE EVADE CODE HERE
        }


        state = NodeState.RUNNING;
        return state;
    }
}
