using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectionZone : MonoBehaviour {

	private static LevelSelectionZone instance;	
	public string zoneGoesTo;

	public string targetLevel = "none";
	
	//Initialization Functions
	public static LevelSelectionZone Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("LevelSelectionZone").AddComponent<LevelSelectionZone>();
			}
			return instance;
		}
	}
	
	//Sets the instance to null when the application quits
	public void OnApplicationQuit()
	{
		instance = null;
	}


	//-----------------------------------------
	//OnTriggerEnter()
	//-----------------------------------------
	//When a collider enters the trigger area stuff happens.
	//-----------------------------------------
	void OnTriggerEnter(Collider other) 
	{
		levelgui lgui = GameObject.FindGameObjectWithTag ("levelgui").GetComponent<levelgui>();


		//this checks to see if the object colliding with the zone is a player
		string t = other.collider.tag;

		//if the zoneGoesTo the level select then the players must be back at the start so it will set that
		//value in each players class equal to true.
		if (zoneGoesTo == "LevelSelect") {
						
						switch (t) {

						case "Player1":
								print ("player1 is in da zone");
								gamestate.Instance.players [0].atStart = true;
								lgui.chickenCheck ();
								break;

						case "Player2":
								print ("player2 is in da zone");
								gamestate.Instance.players [1].atStart = true;
								lgui.chickenCheck ();
								break;

						case "Player3":
								print ("player3 is in da zone");
								gamestate.Instance.players [2].atStart = true;
								lgui.chickenCheck ();
								break;

						case "Player4":
								print ("player4 is in da zone");
								gamestate.Instance.players [3].atStart = true;
								lgui.chickenCheck ();
								break;

						default:
								print ("Thing that entered was not a player.");
								break;
						}
				} else if (zoneGoesTo == "RewardScreen") {
						//if the zoneGoesTo is set to RewardScene then that means that this is at the end of the dungeon so it will
						//set each player to at end
						switch (t) {
				
						case "Player1":
								print ("player1 is in da zone");
								gamestate.Instance.players [0].atEnd = true;
								lgui.victoryCheck();
								break;
						
						case "Player2":
								print ("player2 is in da zone");
								gamestate.Instance.players [1].atEnd = true;
								lgui.victoryCheck();
								break;
						
						case "Player3":
								print ("player3 is in da zone");
								gamestate.Instance.players [2].atEnd = true;
								lgui.victoryCheck();
								break;
						
						case "Player4":
								print ("player4 is in da zone");
								gamestate.Instance.players [3].atEnd = true;
								lgui.victoryCheck();
								break;
						
						default:
								print ("Thing that entered was not a player.");
								break;
						}

				} else {
					switch (t) {
						
						case "Player1":
							print ("player1 is in da zone");
							gamestate.Instance.players [0].isReady = true;
							lgui.readyCheck(zoneGoesTo);
							break;
							
						case "Player2":
							print ("player2 is in da zone");
							gamestate.Instance.players [1].isReady = true;
							lgui.readyCheck(zoneGoesTo);
							break;
							
						case "Player3":
							print ("player3 is in da zone");
							gamestate.Instance.players [2].isReady = true;
							lgui.readyCheck(zoneGoesTo);
							break;
							
						case "Player4":
							print ("player4 is in da zone");
							gamestate.Instance.players [3].isReady = true;
							lgui.readyCheck(zoneGoesTo);
							break;
							
						default:
							print ("Thing that entered was not a player.");
							break;
					}
				}
	}

	//if the thing (player) leaves the zone.
	void OnTriggerExit(Collider other)
	{
		string t = other.collider.tag;
		print ("the player left da zone");

		if (zoneGoesTo == "LevelSelect") {
			
						switch (t) {
				
						case "Player1":
								print ("player1 is in da zone");
								gamestate.Instance.players [0].atStart = false;
								break;
				
						case "Player2":
								print ("player2 is in da zone");
								gamestate.Instance.players [1].atStart = false;
								break;
				
						case "Player3":
								print ("player3 is in da zone");
								gamestate.Instance.players [2].atStart = false;
								break;
				
						case "Player4":
								print ("player4 is in da zone");
								gamestate.Instance.players [3].atStart = false;
								break;
				
						default:
								print ("Thing that entered was not a player.");
								break;
						}
				} else if (zoneGoesTo == "RewardScreen") {
						//if the zoneGoesTo is set to RewardScene then that means that this is at the end of the dungeon so it will
						//set each player to at end
						switch (t) {
				
						case "Player1":
								print ("player1 is in da zone");
								gamestate.Instance.players [0].atEnd = false;
								break;
				
						case "Player2":
								print ("player2 is in da zone");
								gamestate.Instance.players [1].atEnd = false;

								break;
				
						case "Player3":
								print ("player3 is in da zone");
								gamestate.Instance.players [2].atEnd = false;
								break;
				
						case "Player4":
								print ("player4 is in da zone");
								gamestate.Instance.players [3].atEnd = false;
								break;
				
						default:
								print ("Thing that entered was not a player.");
								break;
						}

			
				} else {
					
					switch (t) {
				
						case "Player1":
							print ("player1 is in da zone");
							gamestate.Instance.players [0].isReady = false;
							
							break;
							
						case "Player2":
							print ("player2 is in da zone");
							gamestate.Instance.players [1].isReady = false;
							
							break;
							
						case "Player3":
							print ("player3 is in da zone");
							gamestate.Instance.players [2].isReady = false;
							break;
							
						case "Player4":
							print ("player4 is in da zone");
							gamestate.Instance.players [3].isReady = false;
							break;
							
						default:
							print ("Thing that entered was not a player.");
							break;
						}
				}


	}


	//-----------------------------------------
	//setDestination()
	//-----------------------------------------
	//This sets the destination of the zone
	//-----------------------------------------
	public void setTargetLevel(string dest)
	{
		if (targetLevel != "none") {
			gamestate.Instance.chosenLevel = targetLevel;
		}
	}


	//-----------------------------------------
	//sendDestination()
	//-----------------------------------------
	//This sends the destination to the instance of the gamestate manager
	//-----------------------------------------
	public void sendDestination()
	{

	}

}
