using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class LookAt : AIMovement
{
    public Transform _target;
    public float _angularRotationSpeed;

    public override SteeringOutput GetMovement(AIAgent agent)
    {
        SteeringOutput output = base.GetMovement(agent);

        //output._rotation = Quaternion.RotateTowards(agent.transform.forward, _target.transform.position, _angularRotationSpeed * Time.deltaTime);


        return output;
    }
}
