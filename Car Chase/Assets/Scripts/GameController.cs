using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject collectable;
    private List<GameObject> policeCars = new List<GameObject>();

    public Transform spawnLimitRight;
    public Transform spawnLimitLeft;
    public Transform spawnLimitUp;
    public Transform spawnLimitDown;

    public int collectableCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private int score;
    private bool gameOver;
    private bool restart;

    private float limitRight = 1;
    private float limitLeft = 1;
    private float limitUp = 1;
    private float limitDown = 1;

    void Start()
    {
        if (spawnLimitRight == null) print("spawnLimitRight not set!");
        if (spawnLimitLeft == null) print("spawnLimitLeft not set!");
        if (spawnLimitUp == null) print("spawnLimitUp not set!");
        if (spawnLimitDown == null) print("spawnLimitDown not set!");

        limitRight = spawnLimitRight.position.x;
        limitLeft = spawnLimitLeft.position.x;
        limitUp = spawnLimitUp.position.z;
        limitDown = spawnLimitDown.position.z;

        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";

        var pcars = GameObject.FindGameObjectsWithTag("Seeker");
        if (pcars == null)
        {
            Debug.Log("Can't find any Police Car game objects!!!");
        }
        else
        {
            foreach (var item in pcars)
            {
                policeCars.Add(item);
            }
        }
        
        

        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            for (int i = 0; i < collectableCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(limitLeft, limitRight), 1.0f, Random.Range(limitDown, limitUp));
                Quaternion spawnRotation = Quaternion.identity;

                Instantiate(collectable, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for restart the game or 'Q' for exit the game. ";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int points)
    {
        if (!gameOver)
        {
            score += points;
            UpdateScore();
            foreach (var car in policeCars)
            {
                ExecuteEvents.Execute<IAdjustDifficulty>(car, null, (x, y) => x.AdjustDifficulty(points));
                //Debug.Log("ExecuteEvents.Execute<IAdjustDifficulty> called");
            }
            
            
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
