using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class GenerateMapScript : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Vector3 startPoint;
    private int distanceBetweenNodes;
    private List<GameObject> nodes;
    [SerializeField] private int numOfHorizontalNodes;
    [SerializeField] private GameObject nodeEmptyGameObject;

    public void PlaceNodes()
    {
        DestroyNodes();
        
        distanceBetweenNodes = (Mathf.Abs((int)startPoint.x) * 2)/numOfHorizontalNodes;
        nodes = new List<GameObject>();
        int count = 0;
        for (int i = 0; i < numOfHorizontalNodes*distanceBetweenNodes; i += distanceBetweenNodes)
        {
            for (int j = 0; j < numOfHorizontalNodes*distanceBetweenNodes; j += distanceBetweenNodes)
            {
                for (int x = 0; x < numOfHorizontalNodes*distanceBetweenNodes; x += distanceBetweenNodes)
                {
                    var nodePosition = new Vector3(startPoint.x + i, startPoint.y + j, startPoint.z + x);
                    var newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity);
                    newNode.transform.SetParent(nodeEmptyGameObject.transform);
                    newNode.name = "Node" + (count);
                    nodes.Add(newNode);
                    count++;
                }
            }
        }
        
        
        ConnectNodes();
        VisualizeNodes();
    }

    public void DestroyNodes()
    {
        if (nodes == null || nodes.Count == 0) return;

        foreach (var node in nodes)
        {
            DestroyImmediate(node);
        }
    }

    public void ConnectNodes()
    {
        int runningIndex = 0;
        var maxDistance = Mathf.Sqrt(Mathf.Pow(distanceBetweenNodes, 2) + Mathf.Pow(distanceBetweenNodes, 2)) + 10f;
        
        foreach (var node in nodes)
        {
            foreach (var checkNode in nodes)
            {
                if (Vector3.Magnitude(node.transform.position - checkNode.transform.position) <= maxDistance && checkNode != node)
                {
                    node.GetComponent<GraphNode>().neighbourNodes.Add(checkNode.GetComponent<GraphNode>());
                }
            }
        }
    }

    public void VisualizeNodes()
    {
        foreach (var node in nodes)
        {
            node.GetComponent<GraphNode>().Visualize();
        }
    }
}
