using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

public class PathSmoothing : MonoBehaviour
{
    public int Depth = 3;
    public float CorrectionDistance = 5.0f;

    private Grid grid;

    public void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private Vector2 Vector2FromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + grid.GridWorldSize.x / 2) / grid.GridWorldSize.x;
        float percentY = (worldPosition.z + grid.GridWorldSize.y / 2) / grid.GridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((grid.GridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((grid.GridSizeY - 1) * percentY);
        return new Vector2(x, y);
    }

    private Vector3 Vector2ToWorldPoint(Vector2 gridPosition)
    {
        Vector3 worldPosition = new Vector3(0, 0, 0);
        float percentX = gridPosition.x / (grid.GridSizeX - 1);
        float percentY = gridPosition.y / (grid.GridSizeY - 1);

        worldPosition.x = (percentX * grid.GridWorldSize.x) - (grid.GridWorldSize.x / 2);
        worldPosition.z = (percentY * grid.GridWorldSize.y) - (grid.GridWorldSize.y / 2);

        return worldPosition;
    }

    /// <summary>
    /// This method finds the closest PermittedArea point from the startPosition.
    /// If no permitted area point is found inside the WayPointAdjustingDepth than the startPosition is returned. 
    /// </summary>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    private Vector2 FindClosestPermittedAreaPoint(Vector2 startPosition)
    {
        int x = Mathf.RoundToInt(startPosition.x);
        int y = Mathf.RoundToInt(startPosition.y);

        Vector2 closestPermittedPoint = new Vector2(startPosition.x, startPosition.y);
        bool isFound = false;
        for (int k = 1; k <= Depth; k++)
        {
            //top row
            for (int i = x - k; i <= x + k; i++)
            {
                if (i < 0 || i > grid.GridSizeX - 1) continue;
                if (y < 0 || y > grid.GridSizeY - 1) continue;
                if (grid.GridNodes[i, y + k].PermittedArea)
                {
                    closestPermittedPoint.x = i;
                    closestPermittedPoint.y = y + k;
                    isFound = true;
                    break;
                }
            }

            if (isFound) break;
            //bottom row
            for (int i = x - k; i <= x + k; i++)
            {
                if (i < 0 || i > grid.GridSizeX - 1) continue;
                if (y < 0 || y > grid.GridSizeY - 1) continue;
                if (grid.GridNodes[i, y - k].PermittedArea)
                {
                    closestPermittedPoint.x = i;
                    closestPermittedPoint.y = y - k;
                    isFound = true;
                    break;
                }
            }

            if (isFound) break;
            //left row
            for (int j = y - (k - 1); j <= y + (k - 1); j++)
            {
                if (x < 0 || x > grid.GridSizeX - 1) continue;
                if (j < 0 || j > grid.GridSizeY - 1) continue;
                if (grid.GridNodes[x - k, j].PermittedArea)
                {
                    closestPermittedPoint.x = x - k;
                    closestPermittedPoint.y = j;
                    isFound = true;
                    break;
                }
            }

            if (isFound) break;
            //left row
            for (int j = y - (k - 1); j <= y + (k - 1); j++)
            {
                if (x < 0 || x > grid.GridSizeX - 1) continue;
                if (j < 0 || j > grid.GridSizeY - 1) continue;
                if (grid.GridNodes[x + k, j].PermittedArea)
                {
                    closestPermittedPoint.x = x + k;
                    closestPermittedPoint.y = j;
                    isFound = true;
                    break;
                }
            }

            if (isFound) break;
        }

        return closestPermittedPoint;
    }

    public Vector3[] PathAdjusting(Vector3[] path)
    {
        if (path.Length < 3) return path;

        List<Vector3> newPath = new List<Vector3>();

        newPath.Add(path[0]);
        for (int i = 1; i < path.Length - 2; i++)
        {
            //print("waypoint: (" + path[i].x + "," + path[i].y + "," + path[i].z + ")");
            var tmp = WayPointAdjusting(path[i]);
            newPath.Add(tmp);
            //print("adjusted: (" + tmp.x + "," + tmp.y + "," + tmp.z + ")");
        }
        newPath.Add(path[path.Length - 1]);
        return newPath.ToArray();
    }

    private Vector3 WayPointAdjusting(Vector3 worldPosition)
    {
        var vec2orig = Vector2FromWorldPoint(worldPosition);
        var vec2permitted = FindClosestPermittedAreaPoint(vec2orig);

        if (vec2orig == vec2permitted)
        {
            return worldPosition;
        }

        var vec3permitted = Vector2ToWorldPoint(vec2permitted);

        var direction = new Vector3(worldPosition.x - vec3permitted.x, 0, worldPosition.z - vec3permitted.z);

        float x = ((direction.x / direction.magnitude) * CorrectionDistance) + worldPosition.x;
        float z = ((direction.z / direction.magnitude) * CorrectionDistance) + worldPosition.z;

        var adjustedWorldPosition = new Vector3(x, 0, z);

        return adjustedWorldPosition;
    }
}


