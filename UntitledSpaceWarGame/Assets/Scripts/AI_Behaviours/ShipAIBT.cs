using System.Collections.Generic;
using AI_Behaviours;
using BehaviourTree;
using Unity.VisualScripting;
using Sequence = BehaviourTree.Sequence;

public class ShipAIBT : Tree
{
    public List<UnityEngine.Transform> _waypoints;

    public ShipInformation shipInformation;

    protected override Node SetupTree()
    {
        shipInformation = new ShipInformation(1, 2);
        Node root = new Selector(new List<Node> //REMEMBER: SELECTORS ACT AS "OR" LOGIC GATES
        {
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
                    new Selector(new List<Node>
                    {
                        new Sequence(new List<Node>
                        {
                            new Task_Chase(this)
                        }),
                        new Sequence(new List<Node> //This is where a ship attacks
                        {
                            new Task_Chase(this)
                        })
                    })
                })
            }),
            new Sequence(new List<Node> //REMEMBER: SEQUENCES ACT AS "AND" LOGIC GATES
            {
                new CheckOutOfBounds(this),
                new GoBackInBounds(this)
            }),
            // new Sequence(new List<Node>  
            // { 
            //     new CheckMothershipStatus(this), 
            //     new CheckEnemyCountNearMothership(this),
            //     new Sequence(new List<Node>
            //         {
            //             new GoToMothership(this),
            //             new CheckForAttackRange(this),
            //             new PatrolNearMothership(this, pathfinder)
            //         }
            //     )
            // }),
            new Sequence(new List<Node>
            { 
                new CheckTeammateInNeed(this),
                new HelpTeammate(this)
            }),
            new Sequence(new List<Node>
            { 
                new CheckEnemyInFOV(this),
                new Sequence(new List<Node>
                    {
                        new Task_GoToTarget(this),
                        new CheckForAttackRange(this),
                        new Attack(this)
                    }
                )
            }),
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
    
}