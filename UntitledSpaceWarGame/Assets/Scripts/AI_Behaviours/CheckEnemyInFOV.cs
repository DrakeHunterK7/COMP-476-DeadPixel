﻿using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class CheckEnemyInFOV : Node
    {
        private ShipAIBT ownerShip;

        public CheckEnemyInFOV(ShipAIBT owner)
        {
            ownerShip = owner;
        }

        public override NodeState Evaluate()
        {
            GameObject target = (GameObject) ownerShip.GetRootData("Target");

            if (target != null)
            {
                state = NodeState.SUCCESS;
                return state;
            }





            state = NodeState.FAILURE;
            return state;
        }
    }
}