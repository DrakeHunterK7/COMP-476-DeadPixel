using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class Strafe : AIMovement
{
    public Transform _target; // Need to set this in the behavior tree instead of manually
    private float _distanceToKeep = 30.0f;

    private float _timeUntilRandomDirection = 1.5f;
    private float _currentTime = 0.0f;
    int randomChoice = -1;

    public override SteeringOutput GetMovement(AIAgent agent)
    {
        Vector3 resultingVelocity = Vector3.zero;
        

        SteeringOutput output =  base.GetMovement(agent);
        Vector3 centerOfSphere = _target.position;

        // VELOCITY CODE
        Debug.Log(Vector3.Distance(centerOfSphere, agent.transform.position));
        // Move forward if too far
        if(Vector3.Distance(centerOfSphere, agent.transform.position) > _distanceToKeep * 2)
        {
            resultingVelocity += agent.transform.forward;
        }
        else if(Vector3.Distance(centerOfSphere, agent.transform.position) < _distanceToKeep) // move back if too close
        {
            resultingVelocity -= agent.transform.forward;
        }
        else
        {
            if (randomChoice != -1)
            {
                switch (randomChoice)
                {
                    case 0: // right
                        resultingVelocity += agent.transform.right;
                        break;
                    case 1: // left
                        resultingVelocity -= agent.transform.right;
                        break;
                    case 2: // up
                        resultingVelocity += agent.transform.up;
                        break;
                    case 3: // bottom
                        resultingVelocity -= agent.transform.up;
                        break;
                }
            }
            else
            {
                randomChoice = Random.Range(0, 4);
            }
        }


        if (_currentTime > _timeUntilRandomDirection)
        {
            _currentTime = 0.0f;
            randomChoice = Random.Range(0, 4);
        }
        else
        {
            _currentTime += Time.deltaTime;
        }

        // ROTATION CODE
        Quaternion rotationToApply = Quaternion.LookRotation(centerOfSphere, agent.transform.up);

        // Apply rotation and velocity to 
        output._rotation = rotationToApply;
        //output._velocity = resultingVelocity;

        return output;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

}
