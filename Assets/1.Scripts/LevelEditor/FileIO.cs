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
    private string dummyGameAcctId = "5750085036015616";
    private string dummyLvlId = "6215736263442432";
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
        Application.ExternalCall("reqIds");
        #endif
	}

	public void getIds(string inputIds)
    {
        #if UNITY_EDITOR
		GSManager gsManager = null;
		WWW www = serv.getLvlWww(dummyLvlId);
		try{
			gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		} catch{}
		if(gsManager != null){
		Debug.Log (gsManager);
		Debug.Log ("id: " + gsManager.currLevelId);

		if(gsManager.currLevelId != "") {
			www = serv.getLvlWww(gsManager.currLevelId);
		}
		}
        #else
        string[] ids = inputIds.Split(',');
        gameAcctId = ids[0];
        lvlId = ids[1];
        WWW www = serv.getLvlWww(ids[1]);
        #endif

        StartCoroutine(dlLvl(www));
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
				//throw new Exception();
				MapDataParser.ParseSaveString(lvlData);
            	Debug.Log("LVL DL SUCCESS: " + lvlData);
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
		if(LevelPathCheck.fullPath == null) {
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
		Application.LoadLevel(1);
	}
}
