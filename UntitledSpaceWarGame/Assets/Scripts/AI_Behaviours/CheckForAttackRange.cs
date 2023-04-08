using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForAttackRange : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;
    private Animator _animator;

    public CheckForAttackRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
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

        if (Vector3.Distance(_transform.position, target.position) <= ShipAIBT.attackRange)
        {
            //Changing to attacking animation
            //_animator.SetBool("Attacking", true);
            //_animator.SetBool("Walking", false);
            Debug.Log("Not here");
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}