using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class OrientSelfToTarget : AIMovement
{
    public Transform _target;
    public float _angularRotationSpeed;

    public override SteeringOutput GetMovement(AIAgent agent)
    {
        SteeringOutput output = base.GetMovement(agent);
        Vector3 lookAtVector = _target.transform.position - agent.transform.position;

        output._rotation = Quaternion.LookRotation(lookAtVector, _target.transform.up);

        return output;
    }
}
