using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    private PathRequestManager pathRequestManager;
    private Grid grid;
	public  int extraCost=10; // 
    
    void Awake()
    {
        pathRequestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindingPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine( FindPath(startPosition, targetPosition));
    }

    IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        if(startNode.PermittedArea && targetNode.PermittedArea)
        { 
            Heap<Node> openSet = new Heap<Node>(grid.GridMaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); 
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    //print("Path found: " + sw.ElapsedMilliseconds + " ms.");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.PermittedArea || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    
					if (currentNode.ExtraCost)
						{
						newMovementCostToNeighbour=newMovementCostToNeighbour+extraCost;
						
						}

					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }

                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        pathRequestManager.FinishedProcessingPath(wayPoints, pathSuccess);

    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        Vector3[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);

        return wayPoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if(directionOld != directionNew)
            {
                wayPoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return wayPoints.ToArray();
    }
    
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distanceY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}


