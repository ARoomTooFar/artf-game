//Andrew Miller
//Created 1/17/15
//
//Levelgui.CS provides the bulk of the logic for the game state manager. Here scene transitions will occure when the proper conditions are met
//this script uses functions in the gamestate.cs script.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class levelgui : MonoBehaviour {

	private static gamestate instance;	
	private string levelName;
	private string newName = "Player Name";

	GameObject loadGear;
	GameObject cameras;

	//----------------------------------
	//levelgui()
	//----------------------------------
	//Creates an instance of the gamestate as a gameobject if an instance does not exist
	//----------------------------------
	//public static levelgui Instance
	//{
	//	get
	//	{
	//		if(instance == null)
	//		{
	//			instance = new GameObject("levelgui").AddComponent<levelgui>();
	//		}
	//		return instance;
	//	}	
	//}
	
	//Sets the instance to null when the application quits
	//public void OnApplicationQuit()
	//{
	//	instance = null;
	//}

	// Initialize scene
	void Start () 
	{
		levelName = gamestate.Instance.getLevel ();
		print ("Loaded: " + gamestate.Instance.getLevel ());
		print ("There are still " + gamestate.Instance.getNumPlayers() + " in list");

		//resets the spawn of the player in the new scene
		for(int i = 0; i < gamestate.Instance.getNumPlayers(); i++)
		{	
			if(gamestate.Instance.players[i] != null)
			{
				gamestate.Instance.players[i].transform.position  = new Vector3(0,50,0);	
			}
		}

	}
	
	//--------------------------------
	//OnGUI()
	//--------------------------------
	//Provides scene transition conditions
	//--------------------------------
	public void OnGUI()
	{
		switch (levelName)
		{
			case "PlayerSelect":
				
				break;

			case "LevelSelect":
				
				//Instantiate Players in the scene from the active player list.
				//All players must trigger the dungoeon of their choice.
				//Save this choice as chosenLevel

				//When all have chosen the same dungeon moveToScene("InventorySelection");

				//creates button to move between LevelSelect and InventorySelect
				if (GUI.Button (new Rect (30, 30, 150, 30), "Inventory Select"))
				{
					moveToScene ("InventorySelect");
				}
				break;

			case "InventorySelect":
				//Players can choose a loadout (including an empty one) and modify it.
				//Players can also assign a bait item to their dungeons here.
				//All players ready up to moveToScene("chosenLevel");

				//creates button to move between InventorySelect to the choosen level that was set in the level select area.
				if (GUI.Button (new Rect (30, 30, 150, 30), "Game Scene"))
				{
					moveToScene (gamestate.Instance.getChosenLevel());
				}
				break;

			case "GameScene":

				break;

			case "RewardScene":

				break;

			case "GameOverScene":
				
				break;

			case "Credits":
				break;
		}
	}

	//--------------------------------
	//moveToScene()
	//--------------------------------
	//Moves to specified scene which is passed as a string.
	//--------------------------------
	public void moveToScene(string aScene)
	{
		levelName = aScene;
		gamestate.Instance.resetPartyReady(); //resets the ready up state.
		gamestate.Instance.setPlayerNotReady("all"); //resets all player ready statuses to false.
		print("moving to "+ aScene);
		gamestate.Instance.setLevel(aScene);
		foreach (Player plr in gamestate.Instance.players) { //preserves all the players in the List of player in the game
			if(plr != null)
			{
				plr.atStart = false;
				plr.atEnd = false;
				plr.isReady = false;
				DontDestroyOnLoad(plr);
			}
		}
		//resets the spawn of the player in the new scene
		for(int i = 0; i < gamestate.Instance.getNumPlayers(); i++)
		{	
			if(gamestate.Instance.players[i] != null)
			{
				gamestate.Instance.players[i].transform.position  = new Vector3(0,50,0);	
			}
		}
		loadGear = GameObject.FindGameObjectWithTag("LoadGear");
		cameras = GameObject.FindGameObjectWithTag("GameCameras");

		DontDestroyOnLoad (loadGear);
		DontDestroyOnLoad (cameras);
		DontDestroyOnLoad (gamestate.Instance);
		Application.LoadLevel(aScene);
	}

	//--------------------------------
	//moveToSceneWC()
	//--------------------------------
	//Moves to specified scene which is passed as a string. Preforms a ready check first. If it does not pass it will not move
	//on to the next scene.
	//--------------------------------
	public void moveToSceneWC(string aScene)
	{
		//checks to see if all the players in the game are ready.
		//readyCheck ();
		//if all the players are ready move to the next scene.
		if(gamestate.Instance.getPartyReady()){
			moveToScene(aScene);
		} else {
			print ("Did not pass ready check, make sure everyone is ready");
		}
	}


	//-------------------------------
	//moveToSceneAndQuit()
	//-------------------------------
	//Moves to the tile screen and clears the player lists and resets the game state manager
	//-------------------------------
	public void moveToSceneAndQuit()
	{
		gamestate.Instance.resetPartyReady ();
		gamestate.Instance.setPlayerNotReady ("all");
		print ("Quiting... Moving to title Screen");
		gamestate.Instance.setLevel ("TitleScreen");
		DestroyObject (gamestate.Instance);
		Application.LoadLevel("TitleScreen");
	}



	//-------------------------------
	//readyCheck()
	//-------------------------------
	//Check to see if all the player in the game are ready. Then moves to the next scene.
	//-------------------------------
	public void readyCheck(string ascene)
	{	


		//checks every player living player to see if they are in the victory zone, this victory property in the player class
		//toggled by entering and exiting a zone in the exit portion of the the dungeon.
		bool ready = gamestate.Instance.getReady();
		if (ready) 
		{
			if(ascene == "InventorySelect")
			{
				print ("Did ready check, zone goes to InventorySelect, trying to set choosen level...");
				gamestate.Instance.getChosenLevel();
			}
			print ("The players are ready. Moving on.");

			moveToScene(ascene);
		}else{
			print ("The Players are not prepared...");
		}

	}

	//------------------------------
	//victoryCheck()
	//------------------------------
	//Checks to see if all the players that are alive have reached the end of the dungeon. If so Victory has been achived
	//and all the players in the game will be sent to the Rewards scene.
	//------------------------------

	public void victoryCheck()
	{
		//checks every player living player to see if they are in the victory zone, this victory property in the player class
		//toggled by entering and exiting a zone in the exit portion of the the dungeon.
		bool win = gamestate.Instance.getVictory();
		if (win) 
		{
			print ("The players have completed the dungeon!");
			moveToScene("RewardScreen");
		}else{
			print ("Not all living players have reached the end");
		}
	}

	//------------------------------
	//chickenCheck()
	//------------------------------
	//Checks to see if all the players that are alive have retreated to the beginning of the dungeon. If so they will be returned to
	//the level selection scene.
	//------------------------------
	public void chickenCheck()
	{
		bool chicken = gamestate.Instance.areChicken();
		if(chicken)
		{
			moveToScene("LevelSelect");
		}else{
			print ("Not all living players have retreated to the beginning");
		}
	}

	//-------------------------------
	//gameOverCheck()
	//-------------------------------
	//Checks to see if all the players in the game are dead, if so then it will move on to the game over scene.
	//-------------------------------
	public void gameOverCheck()
	{
		bool allDead = gamestate.Instance.getPartyDead();
		if(allDead)
		{
			print ("All Players are dead");
			moveToScene ("GameOverScreen");
		} else {
			print ("Not all players are dead");
		}
	}
	
}




