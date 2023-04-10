using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForAttackRange : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private ShipAIBT owner;
    private Animator _animator;

    public CheckForAttackRange(ShipAIBT ownerShip)
    {
        owner = ownerShip;
        _animator = owner.transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        
        if (t == null)
        {
            Debug.Log("Here");
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        // if (Vector3.Distance(owner.transform.position, target.position) <= ShipAIBT.attackRange)
        // {
        //     state = NodeState.SUCCESS;
        //     return state;
        // }

        state = NodeState.FAILURE;
        return state;
    }
}
