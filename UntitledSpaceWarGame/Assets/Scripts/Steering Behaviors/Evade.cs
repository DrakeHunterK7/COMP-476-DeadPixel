using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class Evade : AIMovement
{
    public Transform _target;
    private float _lookAheadTime = 1.0f;

    public override SteeringOutput GetMovement(AIAgent agent)
    {
        SteeringOutput output = base.GetMovement(agent);

        // Checker in case the target dies before switching states
        if (_target == null)
        {
            return output;
        }

        // Get Target velocity
        AIAgent targetAgent = _target.GetComponent<AIAgent>();
        Vector3 targetVelocity = targetAgent._velocity;

        // Calculate orientation of agent
        Vector3 targetFuturePosition = _target.transform.position + targetVelocity * _lookAheadTime;
        Vector3 lookAtVector = (agent.transform.position - targetFuturePosition).normalized;

        // Set resulting orientation and velocity (will be forward since Agent is not strafing while pursuing)
        output._rotation = Quaternion.LookRotation(lookAtVector, _target.transform.up);
        output._velocity = agent.transform.forward;

        return output;
    }

    // Call this method in any task that involves chasing a target
    // then add the AIMovement to the current movements using AIAgent.SetActiveMovement()
    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
