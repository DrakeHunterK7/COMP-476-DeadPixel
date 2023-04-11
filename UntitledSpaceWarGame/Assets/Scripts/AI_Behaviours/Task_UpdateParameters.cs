using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using UnityEngine.UIElements;
using AI;
using static UnityEngine.UI.GridLayoutGroup;

public class Task_UpdateParameters : Node
{
    private ShipAIBT _BT;
    
    public Task_UpdateParameters(ShipAIBT ownerShip)
    {
        _BT = ownerShip;
    }

    /* TO-DO
     *  - We need to reset the _currentWaypointIndex to 0 everytime 
     *  - After calculating the path in a Task_Calculate_X_Path script we need to set a bool to make sure that it doesn't calculate it constantly
     */

    public override NodeState Evaluate()
    {
        _BT.aiAgent._maxForwardSpeed = _BT.GetShipData()._movementSpeed;
        
        
        state = NodeState.FAILURE;
        return state;
    }
}
