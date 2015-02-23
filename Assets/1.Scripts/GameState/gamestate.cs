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
	public string chosenLevel; 		//This is the level the players have chosen to play while in level selection.

	private string username1;			//This is the username for the first player
	private string password1;			//This is the password for the first player

	public List<string> usernames = new List<string>();
	public List<string> passwords = new List<string>();
	
	private bool partyReady;				//If all the players in the game are ready

	public List<Player> players = new List<Player>();	//List of the Players in the game so stats can be checked in their player.cs class.
	
	private Player player1;
	private Player player2;
	private Player player3;
	private Player player4;

	private CameraAdjuster cam;
	private Loadgear gear;
	
	private string testUserName;
	
	//----------------------------------
	//gameState()
	//----------------------------------
	//Creates an instance of the gamestate as a gameobject if an instance does not exist. Singleton.
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
	//setChosenLevel()
	//--------------------------------
	//sets the chosen level
	//--------------------------------
	public void setChosenLevel(string ch)
	{
		chosenLevel = ch;
	}


	//--------------------------------
	//addPlayer()
	//--------------------------------
	//Adds a player to the list of players at the index for the relavent player, ie player1 goes into players[0] and so on.
	//--------------------------------
	public void addPlayer(Player newPlayer, int playerNumber)
	{
		//playerNumber - 1 because that will be off by 1.
		print (gamestate.instance.players.Count);
		 print ("player to be added is player" + playerNumber);
		//gamestate.instance.players.Insert((playerNumber - 1),newPlayer);
		gamestate.instance.players [playerNumber - 1] = newPlayer;
	}

	//--------------------------------
	//addTestPlayers()
	//--------------------------------
	//Adds a player to the game. There can only be 4 players in the game at a time. It will not let you add more than 4.
	//takes in in playerNumber which tells where to put the player in the list.
	//--------------------------------
	public void addPlayerToList (int playerNumber)
	{

		//player1 = GameObject.Find ("Player1").GetComponent <Player>();
		gamestate.instance.players.Capacity = 4; //is sure to set the max capacity of the list to 4 (of by one perhaps?).
		switch (playerNumber)
		{
		case 1:
			player1 = GameObject.Find ("Player1").GetComponent <Player>();	
			addPlayer(player1, playerNumber);
			break;
			
		case 2:
			player2 = GameObject.Find ("Player2").GetComponent <Player>();
			addPlayer(player2, playerNumber);
			break;
			
		case 3:
			player3 = GameObject.Find ("Player3").GetComponent <Player>();
			addPlayer(player3, playerNumber);
			break;
			
		case 4:
			player4 = GameObject.Find ("Player4").GetComponent <Player>();
			addPlayer(player4, playerNumber);
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
	public void getUsername(string un, int playerNumber)
	{
		if (un == "")
		{
			print ("Entered Username was empty, please re-enter.");
		} else {
			switch(playerNumber)
			{
			case 1:
				gamestate.instance.username1 = un;
				print("Player " + playerNumber + " Username is " + gamestate.instance.username1 + ".");
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
	public void getPassword(string pw, int playerNumber)
	{
		if (pw == "")
		{
			print ("Entered password was empty, please re-enter.");

		} else {
			switch(playerNumber)
			{
				
			case 1:
				gamestate.instance.username1 = pw;
				print("Player " + playerNumber + " password is " + gamestate.instance.username1 + ".");
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

		//makes sure the list has stuff in it.
		if (gamestate.instance.players != null)
		{


			switch (playerNumber)
			{
				case 1:
					
					if(players[0] != null) 
					{
						gamestate.instance.players[0].isReady = true;
					} else {
						print ("Player at this index is null");
					}
					break;

				case 2:

					if(players[1] != null) 
					{
						gamestate.instance.players[1].isReady = true;
					} else {
						print ("Player at this index is null");
					}
				
					break;

				case 3:
			
					if(players[2] != null) 
					{
						gamestate.instance.players[2].isReady = true;
					} else {
						print ("Player at this index is null");
					}
					break;

				case 4:

					if(players[3] != null) 
					{
						gamestate.instance.players[3].isReady = true;
					} else {
						print ("Player at this index is null");
					}
				
					break;
			default:
				break;
			}
		} else {
			print ("No players to set as ready.");
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
				gamestate.instance.players[0].isReady = false;
				break;
			
			case "2":
				gamestate.instance.players[1].isReady = false;
				break;
			
			case "3":
				gamestate.instance.players[2].isReady = false;
				break;
			
			case "4":
				gamestate.instance.players[3].isReady = false;
				break;
			case "all":
					
			foreach (Player plr in gamestate.instance.players) 
			{
				//if they exist
				if(plr != null)
				{
					plr.isReady = false;
				}
				
			}

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
	
		print ("the party is set to ready");
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


	//loginPlayer()
	//--------------------------------
	//Logs in the player using their information passed in from text feild GUI's. If sucessful it will add a player to the list in their
	//proper position (player 1 is always at gamestate.instance.players[0], etc) it will then set their status to ready.
	//If the login fails it will not add the player to the list and it will not make the player ready.
	//Takes in int playerNumber which tells the GS where to place the logged in player in the players <list>.
	//--------------------------------
	public void loginPlayer(int playerNumber) 
	{
		print ("Tring to login the player");
		//checks to see if the playerNumber is valid. if not says so.
		if(playerNumber > 4 || playerNumber < 1) 
		{
			print ("Player number is not valid");
		}
		//if the login is successful (user name and password are both correct).
		if(true)
		{
			switch (playerNumber)
			{
			case 1: 
				addPlayerToList(playerNumber);
				gamestate.instance.setPlayerReady (playerNumber);
				break;

			case 2: 
				addPlayerToList(playerNumber);
				gamestate.instance.setPlayerReady (playerNumber);
				break;

			case 3: 
				addPlayerToList(playerNumber);
				gamestate.instance.setPlayerReady (playerNumber);
				break;
			
			case 4: 
				addPlayerToList(playerNumber);
				gamestate.instance.setPlayerReady (playerNumber);
				break;
			
			default:
				print ("did not add or make player ready");
				break;
			}

		} else {
			print ("Failed to login");
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
		return gamestate.instance.activeLevel;
	}



	//--------------------------------
	//getChosenLevel()
	//--------------------------------
	//returns the chosen level as a string.
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
			if(plr != null && !plr.isDead)
			{
				//checks to see if the player is at the end of the dungeon
				if(plr.atEnd)
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
	//getReady()
	//--------------------------------
	//checks to see if all the players are ready.
	//--------------------------------
	public bool getReady()
	{
		int ready = 0;
		//checks the player class for each player in list of active players in the game to see if they are ready.
		foreach (Player plr in gamestate.instance.players) 
		{
			if(plr != null && plr.isReady)
			{
				ready++;
			}
		}
		
		print ("There are " + ready + "players ready.");

		if (ready == gamestate.instance.getNumPlayers ()) {
			gamestate.instance.partyReady = true;
			print ("The party is ready");
		} else {
			gamestate.instance.partyReady = false;
			print ("The party is not ready");
		}

		return gamestate.instance.partyReady;
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
			//if they are alive and are in the list
			if(plr != null && !plr.isDead)
			{
				//checks to see if the player is at the start of the dungeon
				if(plr.atStart)
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
		int numPlayers = gamestate.instance.getNumPlayers();
		//checks the player class for each player in list of active players in the game to see if they are alive.
		foreach (Player plr in gamestate.instance.players) 
		{
			if(plr != null && plr.isDead)
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
			if(plr != null && !plr.isDead)
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
	//Checks the players <list> for players, when it finds one (not null) it adds that to the total. Returns an int with the total number of players.
	//--------------------------------
	public int getNumPlayers()
	{
		int numPlayers = 0;
		//gets the number of players in the players list
		foreach (Player plr in gamestate.instance.players) 
		{
			if(plr != null)
			{
				numPlayers++;
				print ("player is added to numPlayers");
			} else {
				print("player index is null");
			}
		}

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
			return gamestate.instance.players[0].isReady;		
			break;
			
		case 2:
			return gamestate.instance.players[1].isReady;	
			break;
			
		case 3:
			return gamestate.instance.players[2].isReady;	
			break;
			
		case 4:
			return gamestate.instance.players[3].isReady;	
			break;
		
		default:
			return false;
			break;
		}
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