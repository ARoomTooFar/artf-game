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
public class FileIO : MonoBehaviour
{
	//Output_TileMap output_tileMap;
	public Button Button_Save = null;
	public Button Button_Deploy = null;
	public Button Button_LoadPlayable = null;
	BinaryWriter bin;
	
	private Farts serv;
	private string lvlData;
	private WWW udLvlReq = null; // WWW request to server for level updating
	
	public string lvlId;
    public string gameAcctId;
	public Text txtDlLvl;
	public Text txtUdLvl;

    #if UNITY_EDITOR
    private string dummyGameAcctId = "123";
	private string dummyLvlId = "4867770441269248";
	#endif
	
	void Start ()
	{
		txtUdLvl.enabled = false;

		Button_Save.onClick.AddListener (() => {
			saveFile (); });

		Button_LoadPlayable.onClick.AddListener (() => {
			LoadPlayable (); });

		
		serv = gameObject.AddComponent<Farts>();



		#if UNITY_EDITOR
		getIds(dummyLvlId);
		#else
		GSManager gsManager = null;
		try{
			gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		} catch{}

		getIds (gsManager.currLevelId);
        #endif
	}

	public void getIds(string inputIds)
    {
		WWW www = null;
        www = serv.getLvlWww(inputIds);

        StartCoroutine(dlLvl(www));
    }

	void makeStretchyWalls(){
		foreach(ARTFRoom room in StretchyWalls.rooms){
			float wallHeight = 5f;

			//get middles of each wall
			Vector3 mid1 = (room.LLCorner + new Vector3(room.URCorner.x, wallHeight, room.LLCorner.z)) / 2f;
			Vector3 mid2 = (room.LLCorner + new Vector3(room.LLCorner.x, wallHeight, room.URCorner.z)) / 2f;
			Vector3 mid3 = (room.URCorner + new Vector3(room.URCorner.x, wallHeight, room.LLCorner.z)) / 2f;
			Vector3 mid4 = (room.URCorner + new Vector3(room.LLCorner.x, wallHeight, room.URCorner.z)) / 2f;

			GameObject wall;
			bool mid1go = true;
			bool mid2go = true;
			bool mid3go = true;
			bool mid4go = true;

			//check which walls have doors
			foreach(SceneryBlock door in room.Doors){
				if(mid1.z == door.Position.z){
					mid1go = false;
				}
				if(mid2.x == door.Position.x){
					mid2go = false;
				}
				if(mid3.x == door.Position.x){
					mid3go = false;
				}
				if(mid4.z == door.Position.z){
					mid4go = false;
				}
			}

			//instantiate the walls without doors
			if(mid1go){
				wall = GameObject.Instantiate(Resources.Load("StretchyWall"), mid1, Quaternion.identity) as GameObject;
				wall.transform.localScale = new Vector3(Mathf.Abs(room.LLCorner.x - room.URCorner.x) + 1, wallHeight, 1f);
			}else{

			}


			if(mid2go){
				wall = GameObject.Instantiate(Resources.Load("StretchyWall"), mid2, Quaternion.identity) as GameObject;
				wall.transform.localScale = new Vector3(1f , wallHeight, Mathf.Abs(room.LLCorner.z - room.URCorner.z) + 1);
			}
			if(mid3go){
				wall = GameObject.Instantiate(Resources.Load("StretchyWall"), mid3, Quaternion.identity) as GameObject;
				wall.transform.localScale = new Vector3(1f, wallHeight, Mathf.Abs(room.LLCorner.z - room.URCorner.z) + 1);
			}
			if(mid4go){
				wall = GameObject.Instantiate(Resources.Load("StretchyWall"), mid4, Quaternion.identity) as GameObject;
				wall.transform.localScale = new Vector3(Mathf.Abs(room.LLCorner.x - room.URCorner.x) + 1, wallHeight, 1f);
			}







		}

//		for(int i = 0; i < StretchyWalls.rooms.Count
	}

    public IEnumerator dlLvl(WWW www)
    {
        yield return www;

        lvlData = www.text;
        Debug.Log(lvlData);
        Debug.Log(www.url);
        if (serv.dataCheck(lvlData))
        {
			txtDlLvl.enabled = false;
			try{
				MapDataParser.ParseSaveString(lvlData);
				Debug.Log("LVL DL SUCCESS: " + lvlData);
				makeStretchyWalls();
				//throw new Exception();
			} catch (Exception ex){
				Debug.Log(ex.Message);
				Debug.Log(ex.StackTrace);
				Debug.Log("ERROR: Map data format wrong. Loading default level.");
				MapDataParser.ParseSaveString(Global.defaultLevel);
			}
        }
        else
        {
            txtDlLvl.text = "LVL DL ERROR: Level ID doesn't exist.";
            Debug.Log("ERROR: Level download failed. " + lvlId + " doesn't exist.");
        }
    }
	
	public void saveFile ()
	{
		if(LevelPathCheck.roomPath == null) {
			return;
		}
        #if UNITY_EDITOR
        udLvlReq = serv.updateLvl(dummyLvlId, dummyGameAcctId, MapData.SaveString, "draftleveldataisbutts");
        #else
        udLvlReq = serv.updateLvl(lvlId, gameAcctId, MapData.SaveString, "draftleveldataisbutts");
        #endif

        txtUdLvl.enabled = true;
	}
	
	void Update()
	{
		// Wait for download to complete
		// Use the returned data from update level request's coroutine
		if (udLvlReq != null)
		{
			if ((udLvlReq.isDone && udLvlReq.error == null))
			{
				txtUdLvl.enabled = false;
				Debug.Log("UPDATE SUCCEEDED: " + udLvlReq.text);
				udLvlReq = null; // Reset lvlUpdate to null so it doesn't the Debug.Log doesn't spam the console every 
			}
			else if (udLvlReq.error != null)
			{
				txtUdLvl.text = "LVL UD ERROR: Level update failed.";
				Debug.Log("ERROR: LEVEL UPDATE FAILED");
				udLvlReq = null;
			}
		}
	}

	void LoadPlayable(){
		Application.LoadLevel("TestLevelSelect");
	}
}
