using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using static UnityEngine.GraphicsBuffer;

/* THIS SCRIPT IS USED FOR STATIC FOLLOWING
 * ONLY APPLY THIS MOVEMENT IF THE TARGET IS NOT MOVING (Nodes/motherships)
 * USED WITH PATHFINDING MOVEMENT
 * 
 */

public class Seek : AIMovement
{
    private Vector3 _targetPosition;
    public Transform _targetTransform;

    public override SteeringOutput GetMovement(AIAgent agent)
    {
        SteeringOutput output =  base.GetMovement(agent);

        _targetPosition = _targetTransform.position;

        Vector3 lookAtVector = (_targetPosition - agent.transform.position).normalized;

        // Apply movement and rotation to AI
        output._rotation = Quaternion.LookRotation(lookAtVector, agent.transform.up);
        output._velocity = agent.transform.forward;

        return output;
    }

    // Sets the target position
    public void SetTargetPosition(Vector3 targetPos)
    {
        _targetPosition = targetPos;
    }

    public Vector3 GetTargetPosition()
    {
        return _targetPosition;
    }
}
