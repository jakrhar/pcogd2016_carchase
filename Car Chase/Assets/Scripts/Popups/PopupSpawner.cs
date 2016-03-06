using UnityEngine;
using System.Collections;

public class PopupSpawner : MonoBehaviour
{
    public GameObject scorePopup;
    public GameObject boostPopup;

    private GameObject player;

    // Use this for initialization
	void Start ()
    {
        if (scorePopup == null)
        {
            Debug.Log("scorePopup GameObject is not set!!!");
        }

        if (boostPopup == null)
        {
            Debug.Log("boostPopup GameObject is not set!!!");
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.Log("player GameObject not found!!!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    /// <summary>
    /// Instantiate a score popup to show the points when player eats a score donut.
    /// </summary>
    /// <param name="popupText">text to be displayed + score value added</param>
    public void InstantiateScorePopup(string popupText)
    {
        var go = GameObject.Instantiate(scorePopup, Camera.main.WorldToScreenPoint(player.transform.position), Quaternion.identity) as GameObject;
        var popup = go.GetComponent<UnityEngine.UI.Text>();
        popup.text = popupText;
        popup.transform.SetParent(transform);
        popup.gameObject.SetActive(true);
    }

    /// <summary>
    /// Instantiate a score popup to show the points when player eats a score donut.
    /// </summary>
    /// <param name="popupText">text to be displayed</param>
    /// <param name="boostTimeInSeconds">boost time in seconds</param>
    public void InstantiateBoostPopup(string popupText, float boostTimeInSeconds)
    {
        var go = GameObject.Instantiate(boostPopup, Camera.main.WorldToScreenPoint(player.transform.position), Quaternion.identity) as GameObject;
        var movementScript = go.GetComponent<BoostPopup>();
        movementScript.popupDelay = boostTimeInSeconds;
        var popup = go.GetComponent<UnityEngine.UI.Text>();
        popup.text = popupText;
        popup.transform.SetParent(transform);
        popup.gameObject.SetActive(true);
    }
}
