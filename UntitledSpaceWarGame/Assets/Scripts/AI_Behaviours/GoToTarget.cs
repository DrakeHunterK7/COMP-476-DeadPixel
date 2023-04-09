using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class GoToTarget : Node
{
    private ShipAIBT owner;
    private Seek seek;

    public GoToTarget(ShipAIBT ownerShip)
    {
        owner = ownerShip;
        seek = owner.transform.GetComponent<Seek>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        
        if (Vector3.Distance(owner.transform.position, target.position) > 0.01f)
        {
            seek.enabled = true;
            seek.AI = owner.transform;
            seek.target = target;
            seek.MaxVelocity = 5f;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
