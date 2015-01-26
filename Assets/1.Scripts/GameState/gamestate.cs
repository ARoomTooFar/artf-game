//Andrew Miller
//Created: 1/17/15
//
//The gamestate.cs script holds the actual game state, and provides acces to it for other scripts.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gamestate : MonoBehaviour {

	//Properties
	private static gamestate instance;	
	private string activeLevel;			//This is the level the players are currently on.
	private string chosenLevel; 		//This is the level the players have chosen to play while in level selection.
	private IList players;				//List of the Players in the game so stats can be checked in their player.cs class.
	private int numPlayersAlive;		//The number of players alive. When this is 0 the players go to the game over scene.
	private bool victory;				//This is true when players reach the end of a dungeon the players will go to the rewards scene.
	private bool chickens;				//If the players return to the enterance of a dungeon they will be sent back to the level selection scene.
	
	//----------------------------------
	//gameState()
	//----------------------------------
	//Creates an instance of the gamestate as a gameobject if an instance does not exist
	//----------------------------------
	public static gamestate Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("gamestate").AddComponent<gamestate>();
			}
			return instance;
		}
	}

	//Sets the instance to null when the application quits
	public void OnApplicationQuit()
	{
		instance = null;
	}
	//INITIALIZATION---------------------------------

	//---------------------------------
	//startState()
	//---------------------------------
	//Creates a new game state
	//---------------------------------
	public void startState()
	{
		print ("creating the start state");
	}

	//------------------------------------------------


	//MUTATOR-FUNCTIONS-------------------------------
	
	//--------------------------------
	//setLevel()
	//--------------------------------
	//Sets the currently active level to a new value
	//--------------------------------
	public void setLevel(string newLevel)
	{
		//Sets the active level to newLevel
		activeLevel = newLevel;
	}


	//--------------------------------
	//addPlayer()
	//--------------------------------
	//Adds a player to the list of players in the game
	//--------------------------------
	public void addPlayer(Player newPlayer)
	{
		players.Add(newPlayer);
	}



	//------------------------------------------------

	//ACCESS-FUNCTIONS--------------------------------
	

	//--------------------------------
	//getLevel()
	//--------------------------------
	//returns the currently active level
	//--------------------------------
	public string getLevel()
	{
		return activeLevel;
	}



	//--------------------------------
	//getChosenLevel()
	//--------------------------------
	//returns the currently active level
	//--------------------------------
	public string getChosenLevel()
	{
		return chosenLevel;
	}



	//--------------------------------
	//getVictory()
	//--------------------------------
	//returns whether the players have reached the end of the dungoen.
	//--------------------------------
	public bool getVictory()
	{
		return victory;
	}



	//--------------------------------
	//getNumPlayersAlive()
	//--------------------------------
	//returns an int with the number of players that are still alive.
	//--------------------------------
	public int getNumPlayersAlive()
	{
		return numPlayersAlive;
	}



	//--------------------------------
	//getNumPlayers()
	//--------------------------------
	//returns an int with the number of players.
	//--------------------------------
	public int getNumPlayers()
	{
		int numPlayers = gamestate.instance.players.Count;
		return numPlayers;
	}

	//--------------------------------
	//areChicken()
	//--------------------------------
	//returns whether the players are chicken.
	//--------------------------------
	public bool areChicken()
	{
		return chickens;
	}







}