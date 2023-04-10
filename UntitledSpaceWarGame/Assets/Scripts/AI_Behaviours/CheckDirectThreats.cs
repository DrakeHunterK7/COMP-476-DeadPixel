using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckDirectThreats : Node
{
    private ShipAIBT owner;
    public CheckDirectThreats(ShipAIBT ownerShip)
    {
        owner = ownerShip;
    }

    public override NodeState Evaluate()
    {
        var willbehit = owner.GetRootData("Target");

        if (willbehit != null)
        {
            Debug.Log("Threat!");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
