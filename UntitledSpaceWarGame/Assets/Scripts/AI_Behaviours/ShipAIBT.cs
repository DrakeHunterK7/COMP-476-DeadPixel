using System.Collections.Generic;
using AI_Behaviours;
using BehaviourTree;
using Unity.VisualScripting;
using Sequence = BehaviourTree.Sequence;

public class ShipAIBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    //Static variables for the nodes to reference
    public static float speed = 100f;
    public static float fovRange = 6f;
    public static float attackRange = 3f;

    protected override Node SetupTree()
    {
        var pathfinder = new AStarPathfinding(gameObject);
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
                        new Evade(this)
                    }),
                    new Sequence(new List<Node> //This is where a ship attacks
                    {
                        new Chase(this)
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
                        new GoToTarget(this),
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
                        new GoToTarget(this),
                        new CheckForAttackRange(this),
                        new Attack(this)
                    }
                )
            }),
            new Patrol(this, pathfinder)
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