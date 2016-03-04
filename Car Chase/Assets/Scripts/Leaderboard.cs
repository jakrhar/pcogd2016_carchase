using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Leaderboard : MonoBehaviour {

    private GameObject gamecontroller;
    public GameObject lb;
    public Text name1, name2, name3, name4, name5, name6, name7, name8, name9, name10;
    public Text score1, score2, score3, score4, score5, score6, score7, score8, score9, score10;
	private GameObject highScoreInfo;
    public InputField nameInput;
    private string newName;
    private int newPos;
	public GameObject gameOverMenu;

    // Use this for initialization
    void Start () {
        gamecontroller = GameObject.FindGameObjectWithTag("GameController");
		highScoreInfo = GameObject.FindGameObjectWithTag ("HighScoreInfo");

        lb = this.gameObject;
        // Hide leaderboard initially
        lb.SetActive(false);
        // Hide name input field initially
		highScoreInfo.SetActive(false);
        nameInput.gameObject.SetActive(false);

        // Load leaderboard content from files on game startup
        UpdateLeaderboard();
	}
    // Save leaderboard content locally to "persistentDataPath/Leaderboard/"
    public void SaveDefault() {
        string[] names = new string[10];
        int[] scores = new int[10];

        names[0] = "DonutDestroyer99";
        scores[0] = 15;
		names[1] = "Trevor Philips";
		scores[1] = 10;
        names[2] = "Master Chief";
        scores[2] = 8;
        names[3] = "Solid Snake";
        scores[3] = 7;
		names[4] = "Lara Croft";
		scores[4] = 6;
		names[5] = "Nathan Drake";
		scores[5] = 5;
		names[6] = "Niko Bellic";
		scores[6] = 4;
		names[7] = "Diabeetus";
		scores[7] = 3;
		names[8] = "Mickey Mouse";
		scores[8] = 2;
		names[9] = "Illusive Man";
		scores[9] = 1;

        BinaryFormatter bf = new BinaryFormatter();
        var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Leaderboard");
        FileStream file = File.Create(Application.persistentDataPath + "/Leaderboard/names.dat");
        FileStream file2 = File.Create(Application.persistentDataPath + "/Leaderboard/scores.dat");

        bf.Serialize(file, names);
        file.Close();
        bf.Serialize(file2, scores);
        file2.Close();
    }
    // Load top 10 names form text file
    public string[] LoadNames(){
        string[] loadedNames = new string[10];
        if (File.Exists(Application.persistentDataPath + "/Leaderboard/scores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Leaderboard/names.dat", FileMode.Open);
            loadedNames = (string[])bf.Deserialize(file);
            file.Close();
        }
        else{
            SaveDefault();
            LoadNames();
        }
        return loadedNames;
    }
    // Load top 10 scores from text file
    public int[] LoadScores(){
        int[] loadedScores = new int[10];
        if (File.Exists(Application.persistentDataPath + "/Leaderboard/scores.dat")){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Leaderboard/scores.dat", FileMode.Open);
            loadedScores = (int[])bf.Deserialize(file);
            file.Close();
        }
        else {
            SaveDefault();
            LoadScores();
        }
        return loadedScores;
    }
    // Load leaderboard content from text files
    public void UpdateLeaderboard () {
        string[] loadedNames = new string[10];
        int[] loadedScores = new int[10];
        if (File.Exists(Application.persistentDataPath + "/Leaderboard/scores.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Leaderboard/names.dat", FileMode.Open);
            FileStream file2 = File.Open(Application.persistentDataPath + "/Leaderboard/scores.dat", FileMode.Open);
            loadedNames = (string[])bf.Deserialize(file);
            file.Close();
            loadedScores = (int[])bf.Deserialize(file2);
            file2.Close();

            // Update leaderboard
            name1.text = loadedNames[0];
            name2.text = loadedNames[1];
            name3.text = loadedNames[2];
            name4.text = loadedNames[3];
            name5.text = loadedNames[4];
            name6.text = loadedNames[5];
            name7.text = loadedNames[6];
            name8.text = loadedNames[7];
            name9.text = loadedNames[8];
            name10.text = loadedNames[9];

            score1.text = loadedScores[0].ToString();
            score2.text = loadedScores[1].ToString();
            score3.text = loadedScores[2].ToString();
            score4.text = loadedScores[3].ToString();
            score5.text = loadedScores[4].ToString();
            score6.text = loadedScores[5].ToString();
            score7.text = loadedScores[6].ToString();
            score8.text = loadedScores[7].ToString();
            score9.text = loadedScores[8].ToString();
            score10.text = loadedScores[9].ToString();
        }
        else {
            SaveDefault();
            UpdateLeaderboard();            
        }
    }
    // Compares final score with the leaderboard scores
    public void CompareScores(int score){
        var currentScores = LoadScores();
        bool found = false;
        for(int i = 0; i < 10; i++){
            if (!found && score >= currentScores[i]){
                newPos = i;
                found = true;
                // Get name from input field
                ShowNameInput();
                nameInput.onEndEdit.AddListener(SubmitName);
            }
        }
    }
    // Save score to leaderboard
    public void SaveScore(int pos, string name){
        // Get score from gamecontroller
        var score = gamecontroller.GetComponent<GameController>().GetScore();

        var names = LoadNames();
        var scores = LoadScores();

		//Shift the array values from the end to make space for the new element
		int i;
		for (i = 8; i >= pos; --i) {
			names [i + 1] = names [i];
			scores [i + 1] = scores [i];
		}
		//Insert new element
        names[pos] = name;
        scores[pos] = score;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Leaderboard/names.dat", FileMode.Open);
        FileStream file2 = File.Open(Application.persistentDataPath + "/Leaderboard/scores.dat", FileMode.Open);

        bf.Serialize(file, names);
        file.Close();
        bf.Serialize(file2, scores);
        file2.Close();

        UpdateLeaderboard();
    }

    // Submit name when pressing enter on input field
    private void SubmitName(string arg)
    {
        Debug.Log(arg);
        newName = arg;
        SaveScore(newPos, newName);
        HideNameInput();
        ShowLeaderboard();
    }

    // Hide/show the name input field
    public void ShowNameInput(){
		gameOverMenu.SetActive (false);
		highScoreInfo.SetActive (true);
        nameInput.gameObject.SetActive(true);
    }
    public void HideNameInput(){
		highScoreInfo.SetActive (false);
        nameInput.gameObject.SetActive(false);
		gameOverMenu.SetActive (true);
    }

    // Hide/show the leaderboard
    public void ToggleLeaderboard(){
        if (lb.activeSelf == true){
            lb.SetActive(false);
        }
        else{
            lb.SetActive(true);
        }
    }
    public void ShowLeaderboard(){
        lb.SetActive(true);
    }
}
