﻿using BehaviourTree;
using UnityEngine;

namespace AI_Behaviours
{
    public class CheckEnemyMotherships : Node
    {
        private ShipAIBT ownerShip;

        public CheckEnemyMotherships(ShipAIBT owner)
        {
            ownerShip = owner;
        }

        public override NodeState Evaluate()
        {
            // GameObject target = (GameObject) ownerShip.GetRootData("Target");
            //
            // if (target != null)
            // {
            //     state = NodeState.SUCCESS;
            //     return state;
            // }
            

            state = NodeState.FAILURE;
            return state;
        }
    }
}