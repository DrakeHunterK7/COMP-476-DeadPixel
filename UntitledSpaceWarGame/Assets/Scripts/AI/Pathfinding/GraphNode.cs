using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphNode : MonoBehaviour
{
    public List<GraphNode> neighbourNodes;

    public float hotCost = 0f;

    private void Update()
    {
        CheckSurroundings();
    }

    void CheckSurroundings()
    {
        Collider[] colliders = new Collider[20];
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, 1000, colliders, LayerMask.GetMask("Ships"));

        hotCost = hitCount * 1000f;
    }

    public void Visualize()
    {
        foreach (var neighbor in neighbourNodes)
        {
            Debug.DrawLine(transform.position, neighbor.transform.position, Color.yellow, 100f);
        }
    }
}
