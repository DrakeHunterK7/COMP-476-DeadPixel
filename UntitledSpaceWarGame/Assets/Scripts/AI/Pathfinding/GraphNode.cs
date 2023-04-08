using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public List<GraphNode> neighbourNodes;

    public void Visualize()
    {
        foreach (var neighbor in neighbourNodes)
        {
            Debug.DrawLine(transform.position, neighbor.transform.position, Color.yellow, 100f);
        }
    }
}
