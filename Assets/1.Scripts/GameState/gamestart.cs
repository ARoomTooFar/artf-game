//Andrew Miller
//Created: 1/17/15
//
//Gamestart.cs initilizes the game state and moves into the first scene in the game.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gamestart : MonoBehaviour {

	//GUI for the title screen
	//void OnGUI()
	//{
		//if(GUI.Button(new Rect(30,30,150,30), "Start"))
		//{
		//	startGame();
		//}
	//}
	
	public void startGame()
	{
		//Initializes Game State Manager
		print ("Starting Game");
		DontDestroyOnLoad (gamestate.Instance);
		gamestate.Instance.startState ();

		for(int i = 0; i < 4; i++)
		{
			gamestate.Instance.players.Add (null);
			gamestate.Instance.usernames.Add (null);
			gamestate.Instance.passwords.Add (null);
			gamestate.Instance.playerChoice.Add (null);

		}
		print(gamestate.Instance.players.Count);
		//Send game off to player selection (Login)
		gamestate.Instance.setLevel("PlayerSelect");
		Application.LoadLevel("PlayerSelect");
	}
}
