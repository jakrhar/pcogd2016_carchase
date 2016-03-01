using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameInput : MonoBehaviour {
    private GameObject leaderboard;

	// Use this for initialization
	void Start () {
        var leaderboard = GameObject.FindGameObjectWithTag("Leaderboard").GetComponent<Leaderboard>();
        var input = gameObject.GetComponent<InputField>();
        input.onEndEdit.AddListener(SubmitName);
    }

    // Submit name when pressing enter
    private void SubmitName(string arg)
    {
        Debug.Log(arg);
        var name = arg;
    }
}
