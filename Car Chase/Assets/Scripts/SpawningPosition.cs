using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is responsible for determining the next spawning postition for any 
/// spawning object in the scene.
/// 
/// Note: The current implementation is based on the pathfinding grid. 
/// It will check from he grid where it is allowed spawn an object. 
/// 
/// </summary>
public class SpawningPosition : MonoBehaviour {

    public float yAxisSpawningValue = 1.0f;
    private GameObject aStar;
    private Vector3[] spawningPositions;
    private IGridInformation gridInfo;
    private bool isCalculated = false;

    /// <summary>
    /// Updates the spawningPositions member with possible spawning positions.
    /// 
    ///     Note: The spawning positions are calculated from the gameworld discretization grid's
    ///           nodes used in pathfinding. The nodes have information if they are walkable or not 
    ///           if they are near obstacles or not.
    /// </summary>
    void Update()
    {
        //do calculation only once
        if (!isCalculated)
        {
            aStar = GameObject.FindGameObjectWithTag("A*");

            if (aStar == null)
            {
                Debug.Log("SpawningPosition.cs -> Failed to find A* gameObject!");
                return;
            }

            gridInfo = aStar.gameObject.GetComponent<Grid>() as IGridInformation;
            if (gridInfo == null)
            {
                Debug.Log("SpawningPosition.cs -> Failed to find Grid component with GetComponent<Grid>()");
                return;
            }

            var possibleSpawningPositions = new List<Vector3>();

            for (int i = 0; i < gridInfo.Heigth; i++)
            {
                for (int j = 0; j < gridInfo.Width; j++)
                {
                    var node = gridInfo.Nodes[i, j];

                    //if the node is permitted area node (where player is able to move) 
                    //and doesn't have extra cost set (currently the extra cost are defines areas near obstacles in the scene)
                    //add the position as possible spawning position.
                    if (node.PermittedArea && !node.ExtraCost)
                    {
                        var vec = node.WorldPosition;
                        vec.y = yAxisSpawningValue; //set y -axis default position 
                        possibleSpawningPositions.Add(vec);
                    }
                }
            }

            spawningPositions = possibleSpawningPositions.ToArray();
            isCalculated = true;
        }

        
    }
	
    /// <summary>
    /// Return the next spawning position of the scene
    /// </summary>
    /// <returns></returns>
    public Vector3 Next()
    {
        return spawningPositions[Random.Range(0, spawningPositions.Length - 1)];
    }
}
