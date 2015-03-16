using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LSZLight : MonoBehaviour {

	//zoneGoesTo tells level selection zone where to take the players NEXT.
	public string zoneGoesTo;
	//finds all the players in the scene, there are always 4 in this demo.
	public Player player1;	
	public Player player2;	
	public Player player3;	
	public Player player4;

	//gamestate manager where the level loading function exists.
	public GSManager gsManager;
	//----------------------------------------------------------------------------------------
	//INITIALIZATION
	//----------------------------------------------------------------------------------------

	//-----------------------------------------
	//Start()
	//-----------------------------------------
	//Sets the players ready state, when all the players are in the ready state it will move back to the 
	//starting screen.
	//-----------------------------------------
	void Start(){

	    player1 = GameObject.Find ("Player1").GetComponent <Player>();	
	 	player2 = GameObject.Find ("Player2").GetComponent <Player>();	
		player3 = GameObject.Find ("Player3").GetComponent <Player>();	
		player4 = GameObject.Find ("Player4").GetComponent <Player>();
		
		//gamestate manager where the level loading function exists.
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();

	}

	//----------------------------------------------------------------------------------------
	//Functions
	//----------------------------------------------------------------------------------------


	//-----------------------------------------
	//OnTriggerEnter()
	//-----------------------------------------
	//Sets the players ready state, when all the players are in the ready state it will move back to the 
	//starting screen.
	//-----------------------------------------
	void OnTriggerEnter(Collider other) 
	{
		//this gets the thing thats colliding
		Player t = other.GetComponent<Player>();
	
		print ("player is in da zone");
		t.isReady = true;
		if(readyCheck())
		{
			gsManager.LoadScene(13);
		}
	}

	//---------------------------------------
	//OnTriggerExit(Collider other)
	//---------------------------------------
	//REsets the players ready state, if they exit the zone.
	//---------------------------------------
	void OnTriggerExit(Collider other)
	{
		//this gets the thing thats colliding
		Player t = other.GetComponent<Player>();	
		print ("player left in da zone");
		t.isReady = false;
		}

	//-------------------------------
	//readyCheck()
	//-------------------------------
	//Check to see if all the player alive in the scene are ready to move on. They are set to ready
	//when they enter the victory zone.
	//-------------------------------
	public bool readyCheck()
	{	
		//number of players living
		int numPlayersAlive = 0;

		//this checks the number of players that are ready to move on.
		int numPlayersReady = 0;

		//these if() statements check to see if each player is alive, if the player is alive it will
		//check if they are ready to move on.
		if(!player1.isDead)
		{
			if(player1.isReady)
			{
				numPlayersReady++;
			}
			numPlayersAlive++;
		}

		if(!player2.isDead)
		{
			if(player2.isReady)
			{
				numPlayersReady++;
			}
			numPlayersAlive++;
		}

		if(!player3.isDead)
		{
			if(player3.isReady)
			{
				numPlayersReady++;
			}
			numPlayersAlive++;
		}

		if(!player4.isDead)
		{
			if(player4.isReady)
			{
				numPlayersReady++;
			}
			numPlayersAlive++;
		}

		//if the number of players that are alive = the number of players that are ready all the players
		// are alive and ready and the game will move on to the next scene.
		if (numPlayersAlive == numPlayersReady) 
		{
			//reset numplayers alive and num players in the game.
			numPlayersAlive = 0;
			print ("All the living players are in the victory zone.");
			return true;
		}else{
			//reset numplayers alive and num players in the game.
			numPlayersAlive = 0;
			print ("Not all of the living players are in the victory zone.");
			return false;
		}
	}
}