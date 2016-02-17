using UnityEngine;
using System.Collections;

/// <summary>
/// This class is responsible for increasing player speed when speed donut is collected.
/// </summary>
public class SpeedCollectableController : MonoBehaviour
{

    public float speedIncrease = 30.0f;
    public float boostTime = 5.0f; 
    public float lifeTime = 5.0f;

    private float time = 0.0f;
    private GameController gameController;
    //private PlayerController playerController;
    //private MovingCharacter playerController;
    //private PlayerMovement playerController;
    private PlayerCarController playerController;
    private bool isBoosting = false;
    private bool isInitialized = false;

    /// <summary>
    /// Find game controller and player game objects
    /// </summary>
    void Awake()
    {
        
    }

    /// <summary>
    /// Destroy speed donut if it's lifetime is over.  
    /// </summary>
    void Update()
    {

        if (!isInitialized)
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
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerCarController>();
            }
            if (playerController == null)
            {
                Debug.Log("Can't find PlayerMovement script!!!");
            }

            isInitialized = true;
        }

        time += Time.deltaTime;
        if (time >= lifeTime && !isBoosting)
        {
            Object.Destroy(gameObject);
        }
    }

    /// <summary>
    /// When player collect the speed donut increase speed temporarily. 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isBoosting)
        {
            //this will move the gameObject away from the scene
            gameObject.transform.position = new Vector3(transform.position.x, -100.0f, transform.position.z);
             
            //start the speed boost
            StartCoroutine(SpeedBoost());
        }
    }

    /// <summary>
    /// This method will increase the players speed temporarily. And then destroy the gameobject. 
    /// </summary>
    /// <returns></returns>
    IEnumerator SpeedBoost()
    {
        isBoosting = true;
        playerController.maxRpm += speedIncrease;

        yield return new WaitForSeconds(boostTime);

        playerController.maxRpm -= speedIncrease;
        Object.Destroy(gameObject);
    }
}
