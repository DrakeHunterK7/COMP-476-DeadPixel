using System.Collections.Generic;
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
                new CheckForAttackRange(transform),
                new Attack(transform),
            }),
            new Sequence(new List<Node>
            { 
                new CheckForEnemy(transform),
                new GoToTarget(transform),
            }),
            //Patrolling is the fallback option if no enemy is in range
            new Patrol(transform, pathfinder),
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