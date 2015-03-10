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
	public Button Button_Load = null;
	BinaryWriter bin;
	
	//static ItemClass itemClass;
	
	private Farts serv;
	private string lvlData = "";
	private WWW udLvlReq = null; // WWW request to server for level updating
	
	public string lvlId;
	public Text txtDlLvl;
	public Text txtUdLvl;
	
	void Start ()
	{

		
		print(lvlId);
		txtUdLvl.enabled = false;
		
		//itemClass = new ItemClass ();
		
		//output_tileMap = GameObject.Find ("TileMap").GetComponent ("Output_TileMap") as Output_TileMap;
		
		//loadingObj = gameObject.AddComponent<LoadingObj>();
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		//		Button_Load.onClick.AddListener (() => {
		//			loadFile ();});
		
		serv = gameObject.AddComponent<Farts>();
		#if UNITY_EDITOR
		getLvlId("5684666375864320");
		#endif
        Application.ExternalCall("reqLvlId", "The game says hello!");
	}

	public void getLvlId(string inputLvlId)
    {
        lvlId = inputLvlId;
		//lvlId = "5684666375864320"; // uncomment this line if you aren't running the level editor in the browser
        WWW www = serv.getLvlWww(inputLvlId);
        StartCoroutine(dlLvl(www));
    }

    public IEnumerator dlLvl(WWW www)
    {
        yield return www;

        lvlData = www.text;
        Debug.Log(lvlData);
        if (serv.dataCheck(lvlData))
        {
            txtDlLvl.enabled = false;
            MapDataParser.ParseSaveString(lvlData);
            Debug.Log("LVL DL SUCCESS: " + lvlData);
        }
        else
        {
            txtDlLvl.text = "LVL DL ERROR: Level ID doesn't exist.";
            Debug.Log("ERROR: Level download failed. " + lvlId + " doesn't exist.");
        }
    }
	
	public void saveFile ()
	{
		udLvlReq = serv.updateLvl(lvlId, "123", MapData.SaveString, "draftleveldataisbutts");
		txtUdLvl.enabled = true;
	}
	
	void Update()
	{
		// Wait for download to complete
		
		// Use the returned data from update level request's coroutine
		if (udLvlReq != null)
		{
			if (udLvlReq.isDone && udLvlReq.error == null)
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
}
