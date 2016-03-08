using UnityEngine;
using System.Collections;

public class UI_Manager : MonoBehaviour {
	
		public bool preventPause;
		private GameObject pausePanel;
		private GameObject gameOverPanel;
		// Use this for initialization
		void Start () {
			pausePanel = GameObject.Find("PauseMenu");
			pausePanel.SetActive(false); //disable pause menu at game start
			
			//gameOverPanel = GameObject.Find("GameOver_Panel");
			//gameOverPanel.SetActive(false); //disable menu at game start
			
		}
		
		void Update ()
		{
			ScanForKeyStroke();
		}
		
		void ScanForKeyStroke()
		{
			if (Input.GetKeyDown("escape") && preventPause == false)
				TogglePauseMenu();
			
		}
		
		/* 
	* A function that sets the time scale and toggles the pause panel in the event
	* of a pause.
	*/
		public void TogglePauseMenu()
		{
			// not the optimal way but for the sake of readability
			if (pausePanel.activeSelf==true)
			{
				pausePanel.SetActive(false);
				Time.timeScale = 1;
				
			}
			else
			{
				pausePanel.SetActive(true);
				Time.timeScale = 0;
			}
			
		}
		
		/* 
	* Brings up Game Over Menu
	*/
		public void GameOverMenu()
		{
			preventPause = true;
			gameOverPanel.SetActive(true);
			
			
		}
		public void QuitGame()
		{
			Application.Quit();
			Debug.Log("Quit to desktop attempted");
		}
		
		// Restarts level after Game Over
		public void GameRestart ()
		{
			//GM.reset=true;
			Application.LoadLevel (Application.loadedLevel);
		}
		
		// Restarts level after Level Finish
		public void LevelRestart ()
		{
			Application.LoadLevel (Application.loadedLevel);
		}
		
		// Load given scene
		public void LoadLevel(string sceneName){
			Application.LoadLevel (sceneName);
		}
	
	

}

