using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public bool DisplayGridGizmos;
    public LayerMask PermittedAreaMask;
    public Vector2 GridWorldSize;
    public float NodeRadius;
    public int extraCostThicknes=1;

    private Node[,] PathfindingGrid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    public int GridSizeX { get { return gridSizeX; } }
    public int GridSizeY { get { return gridSizeY; } }
    public Node[,] GridNodes { get { return PathfindingGrid; } }

    void Awake()
    {
        nodeDiameter = 2 * NodeRadius;
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int GridMaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(PathfindingGrid[checkX, checkY]);
                }

            }
        }

        return neighbours;
    }

    void CreateGrid()
    {
        PathfindingGrid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.forward * (y * nodeDiameter + NodeRadius);
                bool permittedArea = !(Physics.CheckSphere(worldPoint, NodeRadius, PermittedAreaMask));
                PathfindingGrid[x, y] = new Node(permittedArea, worldPoint, x, y,false);
            }
        }
		AddExtraCost();
	}

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float percentY = (worldPosition.z + GridWorldSize.y / 2) / GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return PathfindingGrid[x, y];

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

        if (PathfindingGrid != null && DisplayGridGizmos)
        {
            foreach (Node n in PathfindingGrid)
            {
               // Gizmos.color = n.PermittedArea ? Color.white : Color.red;
             //   Gizmos.DrawCube(n.WorldPosition, Vector3.one *(nodeDiameter - 0.1f));
				Gizmos.color = n.ExtraCost ? Color.yellow : Color.white;
				Gizmos.DrawCube(n.WorldPosition, Vector3.one *(nodeDiameter - 0.1f));
			}
        }
    }   

	public void AddExtraCost()
	{
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				
				
				if (PathfindingGrid[x, y].PermittedArea==false)
					for (int x1 = -1*extraCostThicknes; x1 <= extraCostThicknes; x1++)
				{
					for (int y1 = -1*extraCostThicknes; y1 <= extraCostThicknes; y1++)
					{
						if (x1 == 0 && y1 == 0)
						{
							continue;
						}
						
						int checkX = PathfindingGrid[x,y].GridX + x1;
						int checkY = PathfindingGrid[x,y].GridY + y1;
						
						if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY )
						{
							;
							PathfindingGrid[checkX,checkY].ExtraCost=true;
							
						}
						
					}
					
				}
			}
		}
	}
}

