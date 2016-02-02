using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class leaderboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Save();
        Load();
	}
    // Save leaderboard locally to "persistentDataPath/Leaderboard"
    public void Save () {
        string[] names = new string[10];
        int[] scores = new int[10];

        //names[0] = "Joonas";
        //scores[0] = 100;

        //names[1] = "Laura";
        //scores[1] = 85;

        BinaryFormatter bf = new BinaryFormatter();
        var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Leaderboard");
        FileStream file = File.Create(Application.persistentDataPath + "/Leaderboard/names.dat");
        FileStream file2 = File.Create(Application.persistentDataPath + "/Leaderboard/scores.dat");

        bf.Serialize(file, names);
        file.Close();
        bf.Serialize(file2, scores);
        file2.Close();
    }
    // Load leaderboard from text files
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
            for (int i = 0; i < 10; i++){
                print("name: "+ loadedNames[i] + " score: "+ loadedScores[i]);
            }
        }
        else {
        }
    }
}
