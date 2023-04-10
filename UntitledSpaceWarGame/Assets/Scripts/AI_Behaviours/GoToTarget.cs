using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class GoToTarget : Node
{
    private Transform _transform;
    //private Seek seek;

    public GoToTarget(Transform transform)
    {
        _transform = transform;
        //seek = transform.GetComponent<Seek>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        
        /*if (Vector3.Distance(_transform.position, target.position) > 0.01f)
        {
            seek.enabled = true;
            seek.AI = _transform;
            seek.target = target;
            seek.MaxVelocity = 5f;
        }*/

        state = NodeState.RUNNING;
        return state;
    }
}
