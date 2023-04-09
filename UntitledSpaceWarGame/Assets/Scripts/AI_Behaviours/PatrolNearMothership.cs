using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class PatrolNearMothership : Node
{
    //Patrol attributes
    private ShipAIBT owner;
    private List<Vector3> _waypoints;
    private Animator _animator;

    //Waypoint travel attributes
    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private AStarPathfinding pathfinder;

    public PatrolNearMothership(ShipAIBT ownerShip, AStarPathfinding pathfinding)
    {
        pathfinder = pathfinding;
        owner = ownerShip;
        _animator = owner.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {

        if (_waypoints == null || _waypoints.Count == 0 || _currentWaypointIndex == -1)
        {
            var randomLocation = new Vector3(Random.Range(-10000, 10000), Random.Range(-10000, 10000),
                Random.Range(-10000, 10000));
            _waypoints = pathfinder.AStarLoop(randomLocation);
            
            _currentWaypointIndex = _waypoints.Count - 1;
        }
        else
        {
            Debug.DrawLine(_waypoints[_waypoints.Count-1], _waypoints[0], Color.yellow, 0.1f);
            
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter < _waitTime)
                {
                    _waiting = false;
                    //Walking animation starts
                    //_animator.SetBool("Walking", true);
                }
            }
            else
            {
                Vector3 wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(owner.transform.position, wp) < 0.01f)
                {
                    owner.transform.position = wp;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex--; // = (_currentWaypointIndex + 1) % _waypoints.Length;
                    //Idle animation starts
                    //_animator.SetBool("Walking", false);
                }
                else
                {
                    owner.transform.position = Vector3.MoveTowards(owner.transform.position, wp, ShipAIBT.speed * Time.deltaTime);
                    owner.transform.LookAt(wp);
                }
            }
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}
