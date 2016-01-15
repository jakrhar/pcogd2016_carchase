using UnityEngine;
using System.Collections;

public class CollectableController : MonoBehaviour {

    public int points = 1;
    public float lifeTime = 5.0f;
    private float time = 0.0f; 
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

    void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            Object.Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameController.AddScore(points);
            Object.Destroy(gameObject);
        }

    }
}

