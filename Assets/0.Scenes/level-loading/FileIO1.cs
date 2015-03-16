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
public class FileIO1 : MonoBehaviour
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
    private string dummyLvlId = "5715999101812736";
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
        WWW www = serv.getLvlWww(dummyLvlId);
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
			if (udLvlReq.isDone && udLvlReq.error == null)
			{
				txtUdLvl.enabled = false;
				Debug.Log("UPDATE SUCCEEDED: " + udLvlReq.text);
				udLvlReq = null; // Reset lvlUpdate to null so it doesn't the Debug.Log doesn't spam the console every 

				//destroy all ItemObject scripts on loaded objects
				ItemObject[] itemObs =  FindObjectsOfType(typeof(ItemObject)) as ItemObject[];
				foreach(ItemObject io in itemObs){
					GameObject ob = io.gameObject;
					Destroy (io);
				}
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
		ItemObject[] itemObs =  FindObjectsOfType(typeof(ItemObject)) as ItemObject[];
		foreach(ItemObject io in itemObs){
//			print (io.gameObject.name);
			GameObject ob = io.gameObject;
			DontDestroyOnLoad(ob);
		}
		Application.LoadLevel(1);
	}
}
