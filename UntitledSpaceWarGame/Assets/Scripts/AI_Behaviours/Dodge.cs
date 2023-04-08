using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Dodge : Node
{
    private Transform _transform;
    private ShipAIBT BT;

    public Dodge(Transform transform)
    {
        _transform = transform;
        BT = transform.GetComponent<ShipAIBT>();
    }

    public override NodeState Evaluate()
    {
        _transform.Rotate(0, 0, 1, Space.Self);
        BT.SetRootData("WillbeHit", null);


        state = NodeState.RUNNING;
        return state;
    }
}
