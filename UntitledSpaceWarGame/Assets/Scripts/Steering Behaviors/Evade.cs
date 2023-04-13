using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using TMPro;

public class Evade : AIMovement
{
    public Transform _target;
    private float _lookAheadTime = 1.0f;
    private Vector3 randomUpVector;
    private float rotationTimer = 0f;
    private float steerTimer = 1f;

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
        ShipController targetPlayer = null;

        Vector3 targetVelocity = Vector3.zero; //targetAgent._velocity;
        
        // if the target isn't an Ai but is instead the player
        if (targetAgent == null)
        {
            targetPlayer = _target.GetComponent<ShipController>();
            targetVelocity = targetPlayer.velocity;
        }
        else
        {
            targetVelocity = targetAgent._velocity;
        }

        steerTimer -= Time.deltaTime;
        if (steerTimer < 0)
        {
            steerTimer = 2f;
            if (Random.Range(0, 100) > 10f)
            {
                _lookAheadTime = 700;
            }
            else
            {
                _lookAheadTime = 1.0f;
            }
        }
        

        // Calculate orientation of agent
        Vector3 targetFuturePosition = _target.transform.position + targetVelocity * _lookAheadTime;
        Vector3 lookAtVector = (agent.transform.position - targetFuturePosition).normalized;
        

        // Set resulting orientation and velocity (will be forward since Agent is not strafing while pursuing)
        output._rotation = Quaternion.LookRotation(lookAtVector, randomUpVector);
        output._velocity = agent.transform.forward;

        return output;
    }

    // Call this method in any task that involves chasing a target
    // then add the AIMovement to the current movements using AIAgent.SetActiveMovement()
    public void SetTarget(Transform target)
    {
        _target = target;
        rotationTimer -= Time.deltaTime;
        if (rotationTimer < 0f)
        {
            rotationTimer = 2f;
            if (Random.Range(0, 100) > 75f)
            {
                randomUpVector = Random.Range(0, 100f) > 50 ? _target.transform.right : -target.transform.right;
            }
            else
            {
                randomUpVector = _target.transform.up;
            }
        }
    }
}
