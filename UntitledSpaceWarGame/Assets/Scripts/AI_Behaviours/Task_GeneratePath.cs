using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using AI;
using UnityEngine.UIElements;

public class Task_GeneratePath : Node
{
    Transform _transform;
    AStarPathfinding _pathfinding;
    ShipAIBT _aiBT;

    private char _currentStrategy = 'Z';

    public Task_GeneratePath(Transform transform, ShipAIBT aIBT)
    {
        _transform = transform;
        _pathfinding = new AStarPathfinding(_transform.gameObject);
        _aiBT = aIBT;
    }

    public override NodeState Evaluate()
    {
        var newPathPositionObject = _aiBT.GetRootData("PathfindEndPosition");

        if (newPathPositionObject != null)
        {
            Vector3 destinationPos = (Vector3) newPathPositionObject;
            char newStrategy = (char)_aiBT.GetRootData("NewStrategy");

            // This will update the path if a new strategy is chosen
            if(newStrategy != _currentStrategy)
            {
                List<Vector3> path = _pathfinding.AStarLoop(destinationPos);

                _aiBT.SetRootData("Current_Path", path);
                _aiBT.SetRootData("Current_Path_Index", 0);

                _currentStrategy = newStrategy;
            }
        }
        
       

        state = NodeState.RUNNING;
        return state;
    }
}
