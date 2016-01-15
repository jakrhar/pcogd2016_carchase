using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{
    public bool PermittedArea;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;
    public Node Parent;
	public bool ExtraCost=false;

    public int gCost;
    public int hCost;
    private int heapIndex;

	public Node(bool permittedArea, Vector3 worldPosition, int gridX, int gridY,bool extraCost)
    {
        PermittedArea = permittedArea;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
		ExtraCost = extraCost;
    }
    
    public int fCost { get { return gCost + hCost; } }	

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
