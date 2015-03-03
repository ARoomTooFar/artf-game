using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Runtime.Serialization;
using System.Text;

//This class listens to save/deploy/load buttons
public class UIHandler_FileIO : MonoBehaviour
{
	Output_TileMap output_tileMap;
	public Button Button_Save = null;
	public Button Button_Deploy = null;
	public Button Button_Load = null;
	BinaryWriter bin;

	static ItemClass itemClass;

	private Farts serv;
	long levelId;
	string levelTHING;
	private WWW lvlUpdate; // WWW request to server for level updating

	IEnumerator Start ()
	{
		itemClass = new ItemClass ();
		
		output_tileMap = GameObject.Find ("TileMap").GetComponent ("Output_TileMap") as Output_TileMap;
		
		serv = gameObject.AddComponent<Farts> ();
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
//		Button_Load.onClick.AddListener (() => {
//			loadFile ();});
		

		// Download level example
		WWW dlLvlReq = serv.getLvlWww("levelID");
		yield return dlLvlReq;
		
		// Use the downloaded level data
		string dlLvlData = dlLvlReq.text;
		
		// Check if level data is valid
		if (serv.dataCheck(dlLvlData))
		{
			Debug.Log("DOWNLOAD SUCCEEDED: " + dlLvlData);
			MapDataParser.ParseSaveString(dlLvlData);
		}
		else
		{
			Debug.Log("ERROR: LEVEL DATA DOWNLOAD FAILED");
		}
	}
	
	public void saveFile ()
	{
		lvlUpdate = serv.updateLvl("levelID", "gameaccountID", MapData.SaveString, "draftleveldata");
	}
	

	void Update()
	{
		// Use the returned data from update level request's coroutine
		if (lvlUpdate != null)
		{
			if (lvlUpdate.isDone && lvlUpdate.error == null)
			{
				Debug.Log("UPDATE SUCCEEDED: " + lvlUpdate.text);
				lvlUpdate = null; // Reset lvlUpdate to null so it doesn't the Debug.Log doesn't spam the console every tick
			}
			else if (lvlUpdate.error != null)
			{
				Debug.Log("ERROR: LEVEL UPDATE FAILED");
			}
		}
	}
	
}
