using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectionZone : MonoBehaviour {

	private static LevelSelectionZone instance;	
	public string zoneGoesTo;

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
			//this checks to see if the object colliding with the zone is a player
			if (other.collider.tag == "Player")
			{
				print("player is in da zone");
				//levelgui.movetoscene("InvnetorySelect");
			}
	}

	//-----------------------------------------
	//setDestination()
	//-----------------------------------------
	//This sets the destination of the zone
	//-----------------------------------------
	public void setDestination(string dest)
	{
		zoneGoesTo = dest;
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
