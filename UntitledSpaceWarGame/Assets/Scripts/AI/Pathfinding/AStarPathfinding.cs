using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Analytics;
using Vector3 = UnityEngine.Vector3;

public class AStarPathfinding
{
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private GameObject pathfinder;
    public bool active;
    private LayerMask wallLayer;

    public Vector3 targetActualPosition;
    private Vector3 targetNearestNodePosition;
    public List<Vector3> finalPath;

    public AStarPathfinding(GameObject currentPawn)
    {
        targetActualPosition = Vector3.one * -1000;
        openList = new List<PathNode>();
        closedList = new List<PathNode>();
        pathfinder = currentPawn;
        active = true;
        finalPath = new List<Vector3>();
    }

    float EuclidianDistance(Vector3 startPoint)
    {
        return Vector3.Distance(startPoint, targetActualPosition);
    }

    GraphNode GetNearestGraphNode(Vector3 startPosition)
    {
        GraphNode nearestGNode = null;
        var currentDotProduct = 0f;
        var currentDecidingFactor = 0d;

        Collider[] colliders = new Collider[10];
        var hitCount = Physics.OverlapSphereNonAlloc(startPosition, 2000f, colliders, LayerMask.GetMask("GraphNode"));
        Debug.Log(hitCount);
        for(var i = 0; i < hitCount; i++)
        {
            if (nearestGNode == null)
            {
                nearestGNode = colliders[i].gameObject.GetComponent<GraphNode>();
                if (nearestGNode == null)
                {
                    nearestGNode = colliders[i].gameObject.GetComponentInParent<GraphNode>();
                }
                
                var distance = Vector3.Distance(startPosition, colliders[i].transform.position);

                currentDecidingFactor = distance;
            }
            else
            {
                var distance = Vector3.Distance(startPosition, colliders[i].transform.position);

                var decidingFactor = distance; 
                
                if(decidingFactor < currentDecidingFactor)
                {
                    currentDecidingFactor = decidingFactor;
                    nearestGNode = colliders[i].gameObject.GetComponent<GraphNode>();
                    if (nearestGNode == null)
                    {
                        nearestGNode = colliders[i].gameObject.GetComponentInParent<GraphNode>();
                    }
                }
            }
        }
        return nearestGNode;
    }
    
    static int SortByCost(PathNode n1, PathNode n2)
    {
        return n1.fCost.CompareTo(n2.fCost);
    }
    
    void AddNodeToOpenList(PathNode newNode)
    {
        openList.Add(newNode);
        openList.Sort(SortByCost);
    }

    PathNode GetTopNode()
    {
        return openList[0];
    }

    List<Vector3> CalculatePath(PathNode finalNode)
    {
        var pawnYPos = pathfinder.transform.position.y;
        finalPath.Clear();
        var currentNode = finalNode;
        targetActualPosition.y = pawnYPos;
        finalPath.Add(targetActualPosition);
        while (currentNode != null)
        {
            currentNode.worldPosition.y = pawnYPos;
            finalPath.Add(currentNode.worldPosition);
            currentNode = currentNode.previousNode;
        }

        return finalPath;
    }

    public List<Vector3> AStarLoop(Vector3 location)
    {
        targetActualPosition = location;
        
        GraphNode targetGraphNode = null;
        
        targetGraphNode = GetNearestGraphNode(targetActualPosition);
        
        if (targetGraphNode == null)
        {
            Debug.Log("Target Graph Node Not Found!!");
            return null;
        }

        openList.Clear();
        closedList.Clear();

        GraphNode nearestGraphNode = null;

        nearestGraphNode = GetNearestGraphNode(pathfinder.transform.position);
        
        if (nearestGraphNode == null)
        {
            Debug.Log("Nearest Graph Node Not Found!!");
            return null;
        }
        
        var nearestGraphPosition = nearestGraphNode.gameObject.transform.position;
        var firstNode = new PathNode(nearestGraphPosition);
        firstNode.gCost = Vector3.Magnitude(pathfinder.transform.position-nearestGraphPosition);
        firstNode.hCost = EuclidianDistance(nearestGraphPosition);
        firstNode.fCost = firstNode.gCost + firstNode.hCost;
        firstNode.previousNode = null;
        firstNode.graphNode = nearestGraphNode;
        
        openList.Add(firstNode);

        var isFindingPath = true;
        while (openList.Count != 0 && isFindingPath)
        {
            var topNode = GetTopNode();
            closedList.Add(topNode);

            foreach (var neighbor in topNode.graphNode.neighbourNodes)
            {
                var neighborPosition = neighbor.gameObject.transform.position;

                if (neighborPosition == targetGraphNode.gameObject.transform.position)
                {
                    var newNode = new PathNode(neighborPosition);
                    newNode.previousNode = topNode;
                    newNode.gCost = topNode.gCost + Vector3.Magnitude(topNode.worldPosition - neighborPosition);
                    newNode.hCost = EuclidianDistance(neighborPosition);
                    newNode.fCost = newNode.gCost + firstNode.hCost;
                    newNode.graphNode = neighbor;
                    
                    var calculatedPath = CalculatePath(newNode);
                    return calculatedPath;
                }
                else
                {
                    var newNode = new PathNode(neighborPosition);
                    newNode.previousNode = topNode;
                    newNode.gCost = topNode.gCost + Vector3.Magnitude(topNode.worldPosition - neighborPosition);
                    newNode.hCost = EuclidianDistance(neighborPosition);
                    newNode.fCost = newNode.gCost + firstNode.hCost;
                    newNode.graphNode = neighbor;

                    var shouldAdd = openList.All(node => node.worldPosition != newNode.worldPosition);

                    if (!shouldAdd)
                        continue;
                    
                    shouldAdd = closedList.All(node => node.worldPosition != newNode.worldPosition);

                    if (shouldAdd)
                    {
                        AddNodeToOpenList(newNode);
                    }
                }
            }
            openList.Remove(topNode);
        }

        return null;
    }
    
}
