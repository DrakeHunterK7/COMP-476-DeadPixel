using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector3 worldPosition;
    public PathNode previousNode;
    public float gCost;
    public float hCost;
    public float fCost;
    public GraphNode graphNode;


    public PathNode(Vector3 worldPosition)
    {
        this.worldPosition = worldPosition;
    }
}
