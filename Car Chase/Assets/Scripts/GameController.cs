using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


/// <summary>
/// This class is responsible for the handling the game state.
/// </summary>
public class GameController : MonoBehaviour {

    private List<GameObject> policeCars = new List<GameObject>();

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    public bool isGameOver { get { return gameOver; } }

    private int score;
    private bool gameOver;
    private bool restart;

    /// <summary>
    /// Initializes the game.
    /// Finds the policar instances and adds those to an array.
    /// </summary>
    void Awake()
    {
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
    }

    /// <summary>
    /// When game is over. Read key presses for game restart or exit.
    /// </summary>
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

    /// <summary>
    /// This method will add score for the user.
    /// It will also increase the difficultu -> increase speed of police cars.
    /// </summary>
    /// <param name="points"></param>
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

    /// <summary>
    /// Updates the score to the UI.
    /// </summary>
    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// This method will end the game.
    /// </summary>
    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        restartText.text = "Press 'R' for restart the game or 'Q' for exit the game. ";
        restart = true;
    }
}
