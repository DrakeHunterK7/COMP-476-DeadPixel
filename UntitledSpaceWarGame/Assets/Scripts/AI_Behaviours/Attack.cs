using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Unity.VisualScripting;

public class Attack : Node //@FIXME NEEDS UPDATING FOR DYNAMIC ATTACK PATTERNS (STATIC ATTACK FOR NOW)
{
    private Animator _animator;

    //Reference to enemy (@TODO NEEDS UPDATING FOR ENEMY SHIPS)
    private Transform _lastTarget;

    private ShipAIBT owner;
    //private EnemyManager _enemyManager;

    //Attack variables
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    
    public Attack(ShipAIBT ownerShip)
    {
        _animator = ownerShip.transform.GetComponent<Animator>();
        owner = ownerShip;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if (target != _lastTarget)
        {
            //_enemyManager = target.GetComponent<EnemyManager>();
            _lastTarget = target;
        }

        //Update attack based on speed
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            //Enemy gets hit, lower its health. Remove target from dictionary if enemy dies
            //@TODO - CHECK WHETHER ENEMY IS HIT FROM TARGET SIDE, STOP ATTACKING IF OUT OF RANGE// 
            bool enemyIsDead = true; // = _enemyManager.TakeHit();

            if (enemyIsDead)
            {
                ClearData("target");
                _animator.SetBool("Attacking", false);
                _animator.SetBool("Walking", true);
            }
            else
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
