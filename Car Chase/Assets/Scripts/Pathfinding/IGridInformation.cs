using UnityEngine;
using System.Collections;

/// <summary>
/// This interface describes information of a pathfinding grid.
/// </summary>
public interface IGridInformation
{ 
    /// <summary>
    /// Discribes how big the grid is in world coordinates.
    /// </summary>
    Vector2 WorldSize { get; }

    /// <summary>
    /// Radius of each node in the grid.
    /// </summary>
    float NodeRadius { get; }

    /// <summary>
    /// Node distance of nodes near obstacles that will have extra cost during pathfinding. Discribes the thickness of extra cost area around any obstacles. 
    /// </summary
    int ExtraCostThicknes { get; } 

    /// <summary>
    /// Width of the pathfinding grid == number of nodes in x-axis.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Heigth of the pathfinding grid == number of nodes in y-axis.
    /// </summary>
    int Heigth { get; }

    /// <summary>
    /// Returns all nodes in he grid.
    /// </summary>
    Node[,] Nodes { get; }
}
