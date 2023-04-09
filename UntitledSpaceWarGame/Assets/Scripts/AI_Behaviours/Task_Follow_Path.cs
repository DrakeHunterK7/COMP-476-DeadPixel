using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using UnityEngine.UIElements;
using AI;
using static UnityEngine.UI.GridLayoutGroup;

public class Task_Follow_Path : Node
{
    //Patrol attributes
    private Transform _transform;
    private AIAgent _aiAgent;
    private Seek _seek;

    private ShipAIBT _BT;

    private List<Transform> _waypoints;
    private Animator _animator;

    //Waypoint travel attributes
    private int _currentWaypointIndex;

    private AStarPathfinding pathfinder;

    public Task_Follow_Path(Transform transform, ShipAIBT ownerShip)
    {
        _transform = transform;
        _aiAgent = _transform.GetComponent<AIAgent>();
        _seek = _transform.GetComponent<Seek>();

        _BT = ownerShip;

        _animator = _transform.GetComponent<Animator>();

        pathfinder = new AStarPathfinding(_transform.gameObject);
    }

    /* TO-DO
     *  - To use this task we need to make sure that the path is calculated in another task script (this shouldn't be hard)
     *  - We need to reset the _currentWaypointIndex to 0 everytime 
     *  - After calculating the path in a Task_Calculate_X_Path script we need to set a bool to make sure that it doesn't calculate it constantly
     */

    public override NodeState Evaluate()
    {
        _waypoints = (List<Transform>)_BT.GetRootData("Current_Path");
        _currentWaypointIndex = (int)_BT.GetRootData("Current_Path_Index");

        // Current waypoint that the player is trying to get to
        Transform currentWaypoint = _waypoints[_currentWaypointIndex];

        // AI has arrived at the target position
        if(Vector3.Distance(_transform.position, currentWaypoint.position) < 15.0f)
        {
            _currentWaypointIndex++;
        }

        _seek.SetTargetPosition(currentWaypoint.position);
        _aiAgent.SetActiveMovement(_seek);
        
        state = NodeState.RUNNING;
        return state;
    }
}
