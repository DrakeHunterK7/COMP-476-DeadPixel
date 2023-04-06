using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class CheckForEnemy : Node
{
    //NEED TO BE ADJUSTED FOR ALLY SHIPS
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;
    private Animator _animator;

    public CheckForEnemy(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, ShipAIBT.fovRange);

            if (colliders.Length > 0)
            {
                foreach (Collider c in colliders)
                {
                    if (c.gameObject.CompareTag("Player")) { 
                    parent.parent.SetData("target", c.transform);
                    //Change to animation to running
                    //_animator.SetBool("Running", true);

                    state = NodeState.SUCCESS;
                    return state;}
                }
            }

            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
