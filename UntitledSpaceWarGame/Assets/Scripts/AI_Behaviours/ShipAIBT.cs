using System.Collections.Generic;
using AI_Behaviours;
using BehaviourTree;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = BehaviourTree.Sequence;
using Tree = BehaviourTree.Tree;

public class ShipAIBT : Tree
{
    public List<UnityEngine.Transform> _waypoints;

    public ShipInformation shipInformation;

    protected override Node SetupTree()
    {
        shipInformation = new ShipInformation(Random.Range(0, 3), 2);
        Node root = new Selector(new List<Node> //REMEMBER: SELECTORS ACT AS "OR" LOGIC GATES
        {
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
            new Sequence(new List<Node>
            { 
                new CheckTeammateInNeed(this),
                new HelpTeammate(this)
            }),
            new CheckEnemyInFOV(this),
            new Sequence(new List<Node>
            { 
                new CheckEnemyMotherships(this),
                new Sequence(new List<Node>
                    {
                        new Task_GoToTarget(this),
                        new CheckForAttackRange(this),
                        new Attack(this)
                    }
                )
            }),
            // This should be called after any path that we calculate
            //new Task_Follow_Path(transform, this) // TESTING WITH WAYPOINTS, NEED TO CHANGE AND GENERATE PATHFINDING BASED ON OTHER CONDITIONS
        });

        return root;
    }

    public void SetRootData(string key, object value)
    {
        root.SetData(key, value);
    }
    
    public object GetRootData(string key)
    {
        return root.GetData(key);
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
}