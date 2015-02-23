using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectionZone : MonoBehaviour {

	private static LevelSelectionZone instance;	

	//zoneGoesTo tells level selection zone where to take the players NEXT.
	public string zoneGoesTo;

	//chosen level is what is used to send the players along to the gameplay scene that was specified
	//in the level selection zone.
	public string chosenLevel ;
	
	//Initialization Functions
	//public static LevelSelectionZone Instance
	//{
	//	get
	//	{
	//		if(instance == null)
	//		{
	//			instance = new GameObject("LevelSelectionZone").AddComponent<LevelSelectionZone>();
	//		}
	//		return instance;
	//	}
	//}
	
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
								lgui.victoryCheck ();
								break;
						
						case "Player2":
								print ("player2 is in da zone");
								gamestate.Instance.players [1].atEnd = true;
								lgui.victoryCheck ();
								break;
						
						case "Player3":
								print ("player3 is in da zone");
								gamestate.Instance.players [2].atEnd = true;
								lgui.victoryCheck ();
								break;
						
						case "Player4":
								print ("player4 is in da zone");
								gamestate.Instance.players [3].atEnd = true;
								lgui.victoryCheck ();
								break;
						
						default:
								print ("Thing that entered was not a player.");
								break;
						}

				} else if (zoneGoesTo == "InventorySelect"){
					//if the zone goes to the inventory selection screen this means the zone is in the level selection room.
					//this preforms a ready check and will set the chosen level to the gameplay level they will travel to
					//after inventory selection.
					switch (t) {
				
						case "Player1":
							print ("player1 is in da zone");
							gamestate.Instance.players [0].isReady = true;
							gamestate.Instance.setChosenLevel(chosenLevel);
							lgui.readyCheck(zoneGoesTo);
							break;
							
						case "Player2":
							print ("player2 is in da zone");
							gamestate.Instance.players [1].isReady = true;
							gamestate.Instance.setChosenLevel(chosenLevel);
							lgui.readyCheck(zoneGoesTo);
							break;
							
						case "Player3":
							print ("player3 is in da zone");
							gamestate.Instance.players [2].isReady = true;
							gamestate.Instance.setChosenLevel(chosenLevel);	
							lgui.readyCheck(zoneGoesTo);
							break;
							
						case "Player4":
							print ("player4 is in da zone");
							gamestate.Instance.players [3].isReady = true;
							gamestate.Instance.setChosenLevel(chosenLevel);	
							lgui.readyCheck(zoneGoesTo);
							break;
							
						default:
							print ("Thing that entered was not a player.");
							break;
					}
				
				} else {
				//this lets the zone be used in general if the players are going from one place to another and acts as 
				//a standard ready check. 
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
	//---------------------------------------
	//OnTriggerExit(Collider other)
	//---------------------------------------
	//Looks for a player of each tag, if they are present and they leave the zone their ready is set
	//to false. THIS IS THE EXACTLY THE SAME AS THE ABOVE FUNCTION except all of the values that are set 
	//when the player enters are reset when they leave, along with anything else. Might want to come back and
	//clean these two up later.
	//---------------------------------------
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

			
				}else if (zoneGoesTo == "InventorySelect"){
					//if the zone goes to the inventory selection screen this means the zone is in the level selection room.
					//this will 
				switch (t) {
				
					case "Player1":
						print ("player1 is in da zone");
						gamestate.Instance.players [0].isReady = false;
						gamestate.Instance.setChosenLevel("");	
						break;
						
					case "Player2":
						print ("player2 is in da zone");
						gamestate.Instance.players [1].isReady = false;
						gamestate.Instance.setChosenLevel("");	
						break;
						
					case "Player3":
						print ("player3 is in da zone");
						gamestate.Instance.players [2].isReady = false;
						gamestate.Instance.setChosenLevel("");	
						break;
						
					case "Player4":
						print ("player4 is in da zone");
						gamestate.Instance.players [3].isReady = false;
						gamestate.Instance.setChosenLevel("");	
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
	//chooseLevel()
	//-----------------------------------------
	//This sets the choosen level specified in the prefab to the chosen level of the gamestate manager.
	//if the choosen level has not been set in the gamestate it will set the choosen level in the 
	//state manager to the choosen level of the level selection zone. This will be some string itentifying
	//a dungeon on the arcade machine.
	//-----------------------------------------
	public void chooseLevel()
	{
		print ("The chosen level in gamestate is " + gamestate.Instance.chosenLevel + ". The chosen level of the zone is " + chosenLevel + ".");
		gamestate.Instance.setChosenLevel(chosenLevel);	
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
