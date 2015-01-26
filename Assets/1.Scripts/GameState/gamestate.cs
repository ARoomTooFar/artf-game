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
	private int numPlayers;				//This is the total number of player left in the game.
	private int numPlayersAlive;		//The number of players alive. When this is 0 the players go to the game over scene.
	private bool victory;				//This is true when players reach the end of a dungeon the players will go to the rewards scene.
	private bool chickens;				//If the players return to the enterance of a dungeon they will be sent back to the level selection scene.
	private bool p1ready;				//If the player is ready for the next scene
	private bool p2ready;
	private bool p3ready;
	private	bool p4ready;	
	private bool partyReady;				//If all the players in the game are ready


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

	//--------------------------------
	//setPlayerReady(int playernumber)
	//--------------------------------
	//Set's a player's status to be ready for the next scene
	//--------------------------------

	public void setPlayerReady(int playerNumber)
	{
	
		switch (playerNumber)
		{
			case 1:
				p1ready = true;		
				break;

			case 2:
				p2ready	= true;
				break;

			case 3:
				p3ready = true;
				break;

			case 4:
				p4ready = true;
				break;
		}

	}

	//--------------------------------
	//setPlayerNotReady(int playernumber)
	//--------------------------------
	//Set's a player's status to be ready for the next scene
	//--------------------------------
	
	public void setPlayerNotReady(string playerNumber)
	{
		
		switch (playerNumber)
		{
		case "1":
			p1ready = false;		
			break;
			
		case "2":
			p2ready	= false;
			break;
			
		case "3":
			p3ready = false;
			break;
			
		case "4":
			p4ready = false;
			break;
		case "all":
			p1ready = false;
			p2ready = false;
			p3ready = false;
			p4ready = false;
			print ("reset all player ready values to false");
			break;

		default:
			print ("Not a valit player number in Gamestate.Instance.setPlayerNotReady(string playerNumber)");
			break;
		}
		
	}

	//--------------------------------
	//setPartyReady()
	//--------------------------------
	//sets allReady to true, this means that all the active players in the game are ready to move on.
	//--------------------------------
	public void setPartyReady()
	{
		partyReady = true;
	}

	//--------------------------------
	//resetPartyReady()
	//--------------------------------
	//sets allReady to true, this means that all the active players in the game are ready to move on.
	//--------------------------------
	public void resetPartyReady()
	{
		partyReady = false;
	}

	//--------------------------------
	//setNumberPlayers()
	//--------------------------------
	//sets the total number of active players in the game, there could be a more elegant way of doing this by counting the IList.
	//Should take input from a text field.
	//--------------------------------
	public void setNumberPlayers(string np)
	{
		switch (np) 
		{
		case "1":
			numPlayers = 1;
			break;

		case "2":
			numPlayers = 2;
			break;
		case "3":
			numPlayers = 3;
			break;
		case "4":
			numPlayers = 4;
			break;
		default:
			print ("Not a valid number of players");
			break;
		}
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

	//--------------------------------
	//getPlayerReadyStatus(int playernumber)
	//--------------------------------
	//Get's the players status for the next scene
	//--------------------------------
	
	public bool getPlayerReadyStatus(int playerNumber)
	{
		
		switch (playerNumber)
		{
		case 1:
			return p1ready;		
			break;
			
		case 2:
			return p2ready;	
			break;
			
		case 3:
			return p3ready;	
			break;
			
		case 4:
			return p4ready;	
			break;
		}
		return false;
	}

	//--------------------------------
	//getPartyReady()
	//--------------------------------
	//tells whether the entire party is ready to move on to the next level.
	//--------------------------------
	public bool getPartyReady()
	{
		return partyReady;
	}




}