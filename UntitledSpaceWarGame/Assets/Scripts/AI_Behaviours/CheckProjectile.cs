using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckProjectile : Node
{
    private Transform _transform;
    private ShipAIBT BT;
    public CheckProjectile(Transform transform)
    {
        _transform = transform;
        BT = transform.GetComponent<ShipAIBT>();

    }

    public override NodeState Evaluate()
    {
        var willbehit = BT.GetRootData("WillbeHit");

        if (willbehit != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }
       


        state = NodeState.FAILURE;
        return state;
    }
}
