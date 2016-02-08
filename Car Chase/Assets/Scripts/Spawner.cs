using UnityEngine;
using System.Collections;

/// <summary>
/// This class is responsible for spawning a collectable objects of certain type.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// GameObject to be spawned.
    /// </summary>
    public GameObject collectable; 


    public int collectableCount;
    public float spawnWait;

    /// <summary>
    /// Delay before the spawning begins.
    /// </summary>
    public float startWait;

    public float waveWait;
   
    private SpawningPosition spawningPosition;
    private GameController gameController;

    /// <summary>
    /// Get gameController instance
    /// </summary>
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Can't find gameController script!!!");
        }

        if (collectable == null)
        {
            Debug.Log("GameObject 'collectable' is not set in unity editor!!!");
        }
    }

    /// <summary>
    /// Start spawning Coroutine.
    /// </summary>
    void Awake()
    {
        spawningPosition = GetComponent<SpawningPosition>();

        if (spawningPosition == null)
        {
            Debug.Log("spawningPosition is null!");
        }

        StartCoroutine(SpawnWaves());
    }

    /// <summary>
    /// Spawn collectable gameObjects until game is over.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (!gameController.isGameOver)
        {
            for (int i = 0; i < collectableCount; i++)
            {
                Vector3 spawnPosition = spawningPosition.Next();
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(collectable, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);
        }
    }
}
