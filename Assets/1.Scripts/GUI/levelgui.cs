//Andrew Miller
//Created 1/17/15
//
//Levelgui.CS provides the bulk of the logic for the game state manager. Here scene transitions will occure when the proper conditions are met
//this script uses functions in the gamestate.cs script.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class levelgui : MonoBehaviour {

	private string levelName;
	private string newName = "Player Name";


	// Initialize scene
	void Start () 
	{
		levelName = gamestate.Instance.getLevel ();
		print ("Loaded: " + gamestate.Instance.getLevel ());
		print ("There are still " + gamestate.Instance.getNumPlayers() + " in list");
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
				
				//Players must log in here, choosing a name that matches the list that is synced online.
				//Players must provide a password that also syncs with a list online.
				//Once these are both done add the player to the active list of players in this game session.

				//If they don't have a character, make one with a random name.
				//Let the player choose from a selection of staring items.
				//Give the player a code they can use online to claim the character.
				//Add to the player list (File) and sync with web.
				//Add to the active player list for this session.
				

				//Set # of players. Add a player by pressing the "attack" button on appropriate control station.
				//Ready after longin press "Attack" button.
				
				//If all the players in the game pass the ready check they it will set the party as all ready
				//moveToScene ("LevelSelect");
			

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

				//creates button to move between InventorySelect and LevelSelect
				if (GUI.Button (new Rect (30, 30, 150, 30), "Game Scene"))
				{
					moveToScene ("GameScene");
				}
				break;

			case "GameScene":
				
				//if players go back to the enterance they will return to the level selection scene.
				//if(gamestate.Instance.areChicken())
				//{
				//	moveToScene("levelSelection");
				//}
				
				//checks whether the players have finished the dungeon.
				//if(gamestate.Instance.getVictory())
				//{
				//	moveToScene ("RewardScene");
				//}
				
				//checks whether all the players are dead. If so it will cause a GameOver.
				//if(gamestate.Instance.getNumPlayersAlive() < 1)
				//{
				//	moveToScene ("GameOverScene");
				//}	
				

				//TESTING
				//moveToScene ("GameOverScreen");

				break;

			case "RewardScene":
				//Players vote on items that they want. 
				//When all votes are cast the loot is distributed
				

				//Checks to see if the players want to play another dungeon
				if (GUI.Button (new Rect (30, 90, 150, 30), "Another Dungeon!"))
				{
					moveToScene ("LevelSelect");
				}

				//Checks to see if the players want to quit
				if (GUI.Button (new Rect (30, 30, 150, 30), "Quit"))
				{
					moveToScene ("TitleScreen");
				}
				break;

			case "GameOverScene":
				//Checks to see if the players want to play another dungeon
				//if (GUI.Button (new Rect (30, 90, 150, 30), "Try Another Dungeon!"))
				//{
				//	moveToScene ("LevelSelect");
				//}

				//Checks to see if the players want to play the same dungeon again.
				//if (GUI.Button (new Rect (30, 130, 150, 30), "Try Again!"))
				//{
					//reset players health and item use (may do this automatically)
					
					//gamestate.Instance.getNumPlayersAlive = gamestate.Instance.getNumPlayers;
				//	moveToScene ("LevelSelect");
				//}
			
				//Checks to see if the players want to quit
				//if (GUI.Button (new Rect (30, 30, 150, 30), "Quit"))
				//{
				//	moveToScene ("TitleScreen");
					
				//}
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
		DontDestroyOnLoad (gamestate.Instance);
		Application.LoadLevel(aScene);
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
		Application.LoadLevel("TitleScreen");
	}



	//-------------------------------
	//readyCheck()
	//-------------------------------
	//Check to see if all the player in the game are ready. If so it sets the party status to ready.
	//-------------------------------
	public void readyCheck()
	{	
		//gets the number of players from the GSM
		int numberPlayers = gamestate.Instance.getNumPlayers();
		int readyCount = 0;

		//checks each player to see if they are ready, if they are it will add to the ready count
		if(gamestate.Instance.getPlayerReadyStatus(1) == true)
		{
			readyCount ++;
		}
		if(gamestate.Instance.getPlayerReadyStatus(2) == true)
		{
			readyCount ++;
		}
		if(gamestate.Instance.getPlayerReadyStatus(3) == true)
		{
			readyCount ++;
		}
		if(gamestate.Instance.getPlayerReadyStatus(4) == true)
		{
			readyCount ++;
		}

		//checks to see if the ready count and the player count are the same, if they are then all players are ready.
		if(readyCount == numberPlayers)
		{
			gamestate.Instance.setPartyReady();
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
			moveToScene("Rewards");
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




