//Andrew Miller
//Created: 1/17/15

using UnityEngine;
using System.Collections;

public class gamestate : MonoBehaviour {

	//Properties
	private static gamestate instance;	
	private string activeLevel;			//This is the level the players are currently on.
	private string chosenLevel; 		//This is the level the players have chosen to play while in level selection.
	private IList players;				//List of the players in the game so stats can be checked in their player.cs class.
	private int numPlayers;				//This is the total number of players in the game.
	private int numPlayersAlive;		//The number of players alive. When this is 0 the players go to the game over scene.
	private bool victory;				//This is true when players reach the end of a dungeon the players will go to the rewards scene.
	
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
	//---------------------------------

	//---------------------------------
	//startState()
	//---------------------------------
	//Creates a new game state
	//---------------------------------
	public void startState()
	{
		print ("creating the start state");
	}

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
	//getChosenLevel()
	//--------------------------------
	//returns the currently active level
	//--------------------------------
	public string getChosenLevel()
	{
		return chosenLevel;
	}
	


}