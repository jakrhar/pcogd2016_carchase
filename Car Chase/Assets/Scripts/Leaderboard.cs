using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Leaderboard : MonoBehaviour {

    public GameObject lb;
    public Text name1, name2, name3, name4, name5, name6, name7, name8, name9, name10;

    public Text score1, score2, score3, score4, score5, score6, score7, score8, score9, score10;

    // Use this for initialization
    void Start () {
        lb = this.gameObject;
        // Hide leaderboard initially
        lb.SetActive(false);

        // Load leaderboard content from files on game startup
        Load();
	}
    // Save leaderboard content locally to "persistentDataPath/Leaderboard/"
    public void SaveDefault() {
        string[] names = new string[10];
        int[] scores = new int[10];

        names[0] = "DonutDestroyer99";
        scores[0] = 100;
        names[1] = "AlwaysSecondBest";
        scores[1] = 85;
        names[2] = "Diabeetus";
        scores[2] = 60;

        for(int i = 3; i < 10; i++){
            names[i] = "Player";
            scores[i] = 50;
        }

        BinaryFormatter bf = new BinaryFormatter();
        var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Leaderboard");
        FileStream file = File.Create(Application.persistentDataPath + "/Leaderboard/names.dat");
        FileStream file2 = File.Create(Application.persistentDataPath + "/Leaderboard/scores.dat");

        bf.Serialize(file, names);
        file.Close();
        bf.Serialize(file2, scores);
        file2.Close();
    }
    // Load leaderboard content from text files
    public void Load () {
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
            Load();            
        }
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
}
