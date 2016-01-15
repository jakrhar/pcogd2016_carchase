using UnityEngine;
using System.Collections;

public class SeekerController : MonoBehaviour {

    private GameController gameController;

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
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            //print("Player collided with seeker.");
            gameController.GameOver();
        }
    }
}
