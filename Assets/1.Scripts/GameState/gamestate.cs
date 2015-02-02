//Andrew Miller
//Created: 1/17/15
//
//The gamestate.cs script holds the actual game state, and provides acces to it for other scripts.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class gamestate : MonoBehaviour {

	//Properties
	private static gamestate instance;	

	private string activeLevel;			//This is the level the players are currently on.
	private string chosenLevel; 		//This is the level the players have chosen to play while in level selection.

	private string username1;			//This is the username for the first player
	private string password1;			//This is the password for the first player

	public List<string> usernames = new List<string>();
	public List<string> passwords = new List<string>();

	private int numPlayers;				//This is the total number of player in the game


	private bool p1ready;				//If the player is ready for the next scene
	private bool p2ready;
	private bool p3ready;
	private	bool p4ready;	
	private bool partyReady;				//If all the players in the game are ready

	public List<Player> players = new List<Player>();	//List of the Players in the game so stats can be checked in their player.cs class.

	private Player player1;
	private Player player2;
	private Player player3;
	private Player player4;
	
	private string testUserName;
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

		gamestate.instance.players.Add(newPlayer);
	}

	//--------------------------------
	//addTestPlayers()
	//--------------------------------
	//Adds a player to the game. There can only be 4 players in the game at a time. It will not let you add more than 4.
	//--------------------------------
	public void addPlayerToList ()
	{

		player1 = GameObject.Find ("Player1").GetComponent <Player>();

		switch (gamestate.instance.numPlayers)
		{
		case 0:
			player1 = GameObject.Find ("Player1").GetComponent <Player>();	
			addPlayer(player1);
			gamestate.instance.numPlayers++;
			break;
			
		case 1:
			player2 = GameObject.Find ("Player2").GetComponent <Player>();
			addPlayer(player2);
			gamestate.instance.numPlayers++;
			break;
			
		case 2:
			player3 = GameObject.Find ("Player3").GetComponent <Player>();
			addPlayer(player3);
			gamestate.instance.numPlayers++;
			break;
			
		case 3:
			player4 = GameObject.Find ("Player4").GetComponent <Player>();
			addPlayer(player4);
			gamestate.instance.numPlayers++;
			break;
		
		default:
			print ("Too Many Players.");
			break;
		}

		print ("There are " + gamestate.instance.players.Count + " Players in the Players List");
	}

	//--------------------------------
	//getUsername()
	//--------------------------------
	//gets the username from the UI. Takes in a string that comes from the UI, if it's null it won't assign it to the username variable.
	//will assign un for whichever player is to log in next using the switch statement.
	//--------------------------------
	public void getUsername(string un)
	{
		if (un == "")
		{
			print ("Entered Username was empty, please re-enter.");
		} else {
			switch(gamestate.instance.numPlayers)
			{
			case 1:
				gamestate.instance.username1 = un;
				print("Player " + gamestate.instance.numPlayers + " Username is " + gamestate.instance.username1 + ".");
				break;
				
			case 2:
				print ("No username to place.");
				break;
				
			case 3:
				print ("No username to place.");
				break;
				
			case 4:
				print ("No username to place.");
				break;
				
			default:
				print ("All players acounted for.");
				break;
			}
		}
	}



	//--------------------------------
	//getPassword()
	//--------------------------------
	//gets the password from the UI. Takes in a string that comes from the UI, if it's null it won't assign it to the password variable.
	//will assign the pw to the next player using the switch statement. 
	//--------------------------------
	public void getPassword(string pw)
	{
		if (pw == "")
		{
			print ("Entered password was empty, please re-enter.");

		} else {
			switch(gamestate.instance.numPlayers)
			{
				
			case 1:
				gamestate.instance.username1 = pw;
				print("Player " + gamestate.instance.numPlayers + " password is " + gamestate.instance.username1 + ".");
				break;
				
			case 2:
				print ("No username to place.");
				break;
				
			case 3:
				print ("No username to place.");
				break;
				
			case 4:
				print ("No username to place.");
				break;
				
			default:
				print ("All players acounted for.");
				break;
			}
		}
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
				gamestate.instance.p1ready = true;
				break;

			case 2:
				gamestate.instance.p2ready	= true;
				break;

			case 3:
				gamestate.instance.p3ready = true;
				break;

			case 4:
				gamestate.instance.p4ready = true;
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
			gamestate.instance.p1ready = false;		
			break;
			
		case "2":
			gamestate.instance.p2ready	= false;
			break;
			
		case "3":
			gamestate.instance.p3ready = false;
			break;
			
		case "4":
			gamestate.instance.p4ready = false;
			break;
		case "all":
			gamestate.instance.p1ready = false;
			gamestate.instance.p2ready = false;
			gamestate.instance.p3ready = false;
			gamestate.instance.p4ready = false;
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
		print ("The party is set to ready.");
		gamestate.instance.partyReady = true;
	}

	//--------------------------------
	//resetPartyReady()
	//--------------------------------
	//sets allReady to true, this means that all the active players in the game are ready to move on.
	//--------------------------------
	public void resetPartyReady()
	{
		print ("The party is set to not ready.");
		gamestate.instance.partyReady = false;
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
		return gamestate.instance.activeLevel;
	}



	//--------------------------------
	//getChosenLevel()
	//--------------------------------
	//returns the currently active level
	//--------------------------------
	public string getChosenLevel()
	{
		return gamestate.instance.chosenLevel;
	}



	//--------------------------------
	//getVictory()
	//--------------------------------
	//returns whether the all the players that are alive have reached the end of the dungoen.
	//--------------------------------
	public bool getVictory()
	{

		//gets the number of players that are still alive.
		int alive = gamestate.instance.getNumPlayersAlive ();

		//number of players that are at the end of the dungeon, this property is triggered when a player enters an area in-game.
		int numEnd = 0;

		foreach (Player plr in gamestate.instance.players) 
		{
			//if they are alive
			if(!plr.isDead)
			{
				//use plr.atEnd, when it is added to the player class
				//test end is set to false to test if all the players are not at the end
				if(false)
				{
					numEnd++;
				}
			}

		}

		//if the number of players that are alive is the same as the number of player that are at the end then all the players
		//have reached the end and they win.
		if (alive == numEnd)
		{
			//print ("The players have completed the dungeon.");
			return true;
		} else {
			//print ("The players have not yet completed the dugeon.");
			return false;
		}
	}

	//--------------------------------
	//areChicken()
	//--------------------------------
	//Returns whether the all the players have "chickened out" by leaving the dungeon. Players have
	//a property in the player.cs class that is set to true when they are in the vicinity of enterance of the dungeon.
	//If that property is true in all players that are alive this function will return TRUE. If not it will return FALSE.
	//--------------------------------
	public bool areChicken()
	{
		
		//gets the number of players that are still alive.
		int alive = gamestate.instance.getNumPlayersAlive ();
		
		//number of players that are at the end of the dungeon, this property is triggered when a player enters an area in-game.
		int numBegn = 0;
		
		foreach (Player plr in gamestate.instance.players) 
		{
			//if they are alive
			if(!plr.isDead)
			{
				//replace with plr.atBegin when its added to the gamestate
				//set to true for test
				if(true)
				{
					numBegn++;
				}
			}
			
		}
		
		//if the number of players that are alive is the same as the number of player that are at the beginning then all the players
		//have chichened out.
		if (alive == numBegn)
		{
			print ("The players are fleeing the dungoen.");
			return true;
		} else {
			print ("The players are still running the dungeon");
			return false;
		}
	}

	//--------------------------------
	//getPartyAlive()
	//--------------------------------
	//returns true if all players are dead, returns false if there is still player alive.
	//--------------------------------
	public bool getPartyDead()
	{
		
		int dead = 0;
		int numPlayers = gamestate.instance.players.Count;
		//checks the player class for each player in list of active players in the game to see if they are alive.
		foreach (Player plr in gamestate.instance.players) 
		{
			if(plr.isDead)
			{
				dead++;
			}
		}
		print ("There are " + numPlayers + " Players");
		print ("There are " + dead + " Dead Players");
		
		if (dead == numPlayers) 
		{
			print ("All the players are dead :(.");
			return true;
		} else {
			print ("There are still players alive.");
			return false;
		}
		
	}
	
	//--------------------------------
	//getNumPlayersAlive()
	//--------------------------------
	//returns an int with the number of players that are still alive.
	//--------------------------------
	public int getNumPlayersAlive()
	{
		int alive = 0;
		//checks the player class for each player in list of active players in the game to see if they are alive.
		foreach (Player plr in gamestate.instance.players) 
		{
			if(!plr.isDead)
			{
				alive++;
			}
		}

		print ("There are " + alive + "players alive. From gamestate.getNumPlayersAlive()");
		return alive;
	}




	//--------------------------------
	//getNumPlayers()
	//--------------------------------
	//returns an int with the number of players.
	//--------------------------------
	public int getNumPlayers()
	{
		//gets the number of players in the players list
		int numPlayers = gamestate.instance.players.Count;
		print ("There are " + numPlayers + "in the game");
		return numPlayers;
	}



	//--------------------------------
	//getPlayerReadyStatus(int playernumber)
	//--------------------------------
	//Gets the players status for the next scene
	//--------------------------------
	
	public bool getPlayerReadyStatus(int playerNumber)
	{
		
		switch (playerNumber)
		{
		case 1:
			return gamestate.instance.p1ready;		
			break;
			
		case 2:
			return gamestate.instance.p2ready;	
			break;
			
		case 3:
			return gamestate.instance.p3ready;	
			break;
			
		case 4:
			return gamestate.instance.p4ready;	
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
		//debugging stuff
		if(gamestate.instance.partyReady){
			print("Party is ready");
		}else{
			print ("Party is not ready");
		}
		return gamestate.instance.partyReady;
	}
	
}