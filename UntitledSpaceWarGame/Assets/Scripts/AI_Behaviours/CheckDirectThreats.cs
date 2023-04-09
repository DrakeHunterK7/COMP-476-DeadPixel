using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckDirectThreats : Node
{
    private ShipAIBT owner;
    private ShipAIBT BT;
    public CheckDirectThreats(ShipAIBT ownerShip)
    {
        owner = ownerShip;
        BT = owner.gameObject.GetComponent<ShipAIBT>();
    }

    public override NodeState Evaluate()
    {
        var willbehit = BT.GetRootData("Target");

        if (willbehit != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
