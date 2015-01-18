//Andrew Miller
//Created: 1/17/15

using UnityEngine;
using System.Collections;

public class gamestart : MonoBehaviour {

	//GUI for the title screen
	void OnGUI()
	{
		if(GUI.Button(new Rect(30,30,150,30), "Start"))
		{
			startGame();
		}
	}
	
	private void startGame()
	{
		//Initializes Game State Manager
		print ("Starting Game");
		DontDestroyOnLoad (gamestate.Instance);
		gamestate.Instance.startState ();

		//Send game off to player selection (Login)
		gamestate.Instance.setLevel("PlayerSelect");
		Application.LoadLevel("PlayerSelect");
	}
}
