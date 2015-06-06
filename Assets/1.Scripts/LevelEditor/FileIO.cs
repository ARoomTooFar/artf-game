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
public class FileIO : MonoBehaviour {
	public Button Button_Save = null;
	public Button Button_Deploy = null;
	public Button Button_LoadPlayable = null;
	private GSManager gsManager;
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
	
	void Start() {
		txtUdLvl.enabled = false;

		Button_Save.onClick.AddListener(() => {	saveFile(); });

		Button_LoadPlayable.onClick.AddListener(() => {	LoadPlayable(); });

		gsManager = null;
		try {
			gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		} catch {}
		serv = gameObject.AddComponent<Farts>();

		#if UNITY_EDITOR
		if (gsManager == null || gsManager.currLevelId == "") {
			Debug.Log ("DUMMY CALLED");
			getIds(dummyLvlId);
		} else {
			Debug.Log ("LVLLOADED");
			getIds (gsManager.currLevelId);
		}
		#elif UNITY_WEBPLAYER
		Application.ExternalCall("reqIds");
		#else
		getIds(gsManager.currLevelId);
		#endif
	}

	public void getIds(string inputIds) {
		#if UNITY_WEBPLAYER
		string[] ids = inputIds.Split(',');
		gameAcctId = ids[0];
		lvlId = ids[1];
		WWW www = serv.getLvlWww(ids[1]);
		#else
		WWW www = null;
		www = serv.getLvlWww(inputIds);
		#endif

		StartCoroutine(dlLvl(www));
	}
	
	public IEnumerator dlLvl(WWW www) {
		yield return www;

		lvlData = www.text;
		Debug.Log(lvlData);
		Debug.Log(www.url);
		if(serv.dataCheck(lvlData)) {
			txtDlLvl.enabled = false;
			try {
				MapDataParser.ParseSaveString(lvlData);
				Debug.Log("LVL DL SUCCESS: " + lvlData);
				//throw new Exception();
			} catch(Exception ex) {
				Debug.Log(ex.Message);
				Debug.Log(ex.StackTrace);
				Debug.Log("ERROR: Map data format wrong. Loading default level.");
				MapDataParser.ParseSaveString(Global.defaultLevel);
			}
		} else {
			txtDlLvl.text = "LVL DL ERROR: Level ID doesn't exist.";
			Debug.Log("ERROR: Level download failed. " + lvlId + " doesn't exist.");
		}
	}
	
	public void saveFile() {
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
	
	void Update() {
		// Wait for download to complete
		// Use the returned data from update level request's coroutine
		if(udLvlReq != null) {
			if((udLvlReq.isDone && udLvlReq.error == null)) {
				txtUdLvl.enabled = false;
				Debug.Log("UPDATE SUCCEEDED: " + udLvlReq.text);
				udLvlReq = null; // Reset lvlUpdate to null so it doesn't the Debug.Log doesn't spam the console every 
			} else if(udLvlReq.error != null) {
				txtUdLvl.text = "LVL UD ERROR: Level update failed.";
				Debug.Log("ERROR: LEVEL UPDATE FAILED");
				udLvlReq = null;
			}
		}
	}

	void LoadPlayable() {
		Application.LoadLevel("TestLevelSelect");
	}
}
