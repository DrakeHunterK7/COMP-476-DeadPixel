using System;
using System.Collections.Generic;
using AI;
using AI_Behaviours;
using BehaviourTree;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = BehaviourTree.Sequence;
using Tree = BehaviourTree.Tree;

public class ShipAIBT : Tree
{
    public ShipInformation shipInformation;
    public Mothership mothership;
    public AIAgent aiAgent;
    public GameObject _shootpoint;
    public GameObject _bulletPrefab;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> //REMEMBER: SELECTORS ACT AS "OR" LOGIC GATES
        {
            new Task_UpdateParameters(this),
            new Sequence(new List<Node> //REMEMBER: SEQUENCES ACT AS "AND" LOGIC GATES
            {
                new CheckOutOfBounds(this),
                new GoBackInBounds(this)
            }),
            new Sequence(new List<Node> //REMEMBER: SEQUENCES ACT AS "AND" LOGIC GATES
            {
                new CheckDirectThreats(this),
                new Selector(new List<Node>
                {
                    new Sequence(new List<Node> 
                    {
                        new CheckDistanceFromThreat(this),
                        new Task_Evade(this)
                    }),
                    new Task_Chase(this)
                })
            }),
            new CheckEnemyInFOV(this),
            new Sequence(new List<Node> 
            {
                new Task_DecideBestStrategy(transform, shipInformation, this),
                new Task_GeneratePath(transform, this),
                new Task_Follow_Path(transform, this)
            })
        });
        return root;
    }
    
    public void Shoot()
    {
        var forwarddir = (_shootpoint.transform.position - transform.position).normalized;
        
        var b = Instantiate(_bulletPrefab, _shootpoint.transform.position, transform.rotation);
        b.GetComponent<Projectile>().direction = forwarddir;
        b.GetComponent<Projectile>().ownerShip = gameObject;
        b.GetComponent<Gun>()._attackStrength = GetShipData().GetAttackForce();
    }

    public void SetRootData(string key, object value)
    {
        root.SetData(key, value);
    }
    
    public object GetRootData(string key)
    {
        return root.GetData(key);
    }
    
    public object ClearRootData(string key)
    {
        return root.ClearData(key);
    }

    public ShipInformation GetShipData()
    {
        return shipInformation;
    }

    public void SetShipData(int team, int shipType)
    {
        shipInformation = new ShipInformation(team, shipType);
    }
    
    public void SetShipData(ShipInformation newShipInfo)
    {
        shipInformation = new ShipInformation(newShipInfo);
    }

    private void OnDestroy()
    {
        // GameObject target = (GameObject) GetRootData("Target");
        //
        // if (target != null)
        // {
        //     target.GetComponent<ShipAIBT>().ClearRootData("Target");
        //     ClearRootData("Target");
        // }
    }
}