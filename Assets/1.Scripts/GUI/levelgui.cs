//Andrew Miller
//Created 1/17/15

using UnityEngine;
using System.Collections;

public class levelgui : MonoBehaviour {

	private string levelName;
	private string newName = "Player Name";

	// Initialize scene
	void Start () 
	{
		levelName = gamestate.Instance.getLevel ();
		print ("Loaded: " + gamestate.Instance.getLevel ());
	}
	
	//--------------------------------
	//OnGUI()
	//--------------------------------
	//Provides a GUI in scenes
	//--------------------------------
	void OnGUI()
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
				

				//Set # of players. Add a player by pressing the "attack" button on station.
				//Ready after longin press "Attack" button.
		
				//newName = GUI.TextField(new Rect(30, 100, 150, 30), newName, 25);

				//creates button to move between playerselect and levelselect and only if a number of players has been added.
				if (GUI.Button (new Rect (30, 30, 150, 30), "Level Select")) 
				{
					moveToScene ("LevelSelect");
				}
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
				if(gamestate.Instance.areChicken())
				{
					moveToScene("levelSelection");
				}
				
				//checks whether the players have finished the dungeon.
				if(gamestate.Instance.getVictory())
				{
					moveToScene ("RewardScene");
				}
				
				//checks whether all the players are dead. If so it will cause a GameOver.
				if(gamestate.Instance.getNumPlayersAlive() < 1)
				{
					moveToScene ("GameOverScene");
				}	
				
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
				if (GUI.Button (new Rect (30, 90, 150, 30), "Try Another Dungeon!"))
				{
					moveToScene ("LevelSelect");
				}

				//Checks to see if the players want to play the same dungeon again.
				if (GUI.Button (new Rect (30, 130, 150, 30), "Try Again!"))
				{
					//reset players health and item use (may do this automatically)
					
					//gamestate.Instance.getNumPlayersAlive = gamestate.Instance.getNumPlayers;
					moveToScene ("LevelSelect");
				}
			
				//Checks to see if the players want to quit
				if (GUI.Button (new Rect (30, 30, 150, 30), "Quit"))
				{
					moveToScene ("TitleScreen");
					
				}
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
	void moveToScene(string aScene)
	{
		levelName = aScene;
		print("moving to "+ aScene);
		gamestate.Instance.setLevel(aScene);
		DontDestroyOnLoad (gamestate.Instance);
		Application.LoadLevel(aScene);
	}

}
