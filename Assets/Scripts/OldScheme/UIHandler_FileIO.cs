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
//	Output_TileMap output_tileMap;
//	public Button Button_Save = null;
//	public Button Button_Deploy = null;
//	public Button Button_Load = null;
//	BinaryWriter bin;
//
//	static ItemClass itemClass;
//
//	private Farts serv;
//    private string lvlData = "";
//    private WWW udLvlReq = null; // WWW request to server for level updating
//
//    public string lvlId = "0";
//    public Text txtDlLvl;
//    public Text txtUdLvl;
//
//	IEnumerator Start ()
//	{
//        txtUdLvl.enabled = false;
//
//		itemClass = new ItemClass ();
//		
//		output_tileMap = GameObject.Find ("TileMap").GetComponent ("Output_TileMap") as Output_TileMap;
//		
//        //loadingObj = gameObject.AddComponent<LoadingObj>();
//		Button_Save.onClick.AddListener (() => {
//			saveFile (); });
////		Button_Load.onClick.AddListener (() => {
////			loadFile ();});
//
//        serv = gameObject.AddComponent<Farts>();
//
//        // Need to get the level ID from browser, but for now we're cheezin it
//        lvlId = "5684666375864320";
//
//        // Download the level
//        WWW dlLvlReq = serv.getLvlWww(lvlId);
//        yield return dlLvlReq;
//
//        // Use the downloaded level data
//        lvlData = dlLvlReq.text;
//        Debug.Log(lvlData);
//        if (serv.dataCheck(lvlData))
//        {
//            txtDlLvl.enabled = false;
//            MapDataParser.ParseSaveString(lvlData);
//            Debug.Log("LVL DL SUCCESS: " + lvlData);
//        }
//        else
//        {
//            txtDlLvl.text = "LVL DL ERROR: Level ID doesn't exist.";
//            Debug.Log("ERROR: Level download failed. " + lvlId + " doesn't exist.");
//        }
//	}
//	
//	public void saveFile ()
//	{
//        udLvlReq = serv.updateLvl("5684666375864320", "123", MapData.SaveString, "draftleveldataisbutts");
//        txtUdLvl.enabled = true;
//	}	
//
//	void Update()
//	{
//        // Wait for download to complete
//
//		// Use the returned data from update level request's coroutine
//        if (udLvlReq != null)
//		{
//            if (udLvlReq.isDone && udLvlReq.error == null)
//            {
//                txtUdLvl.enabled = false;
//                Debug.Log("UPDATE SUCCEEDED: " + udLvlReq.text);
//                udLvlReq = null; // Reset lvlUpdate to null so it doesn't the Debug.Log doesn't spam the console every 
//            }
//            else if (udLvlReq.error != null)
//			{
//                txtUdLvl.text = "LVL UD ERROR: Level update failed.";
//                Debug.Log("ERROR: LEVEL UPDATE FAILED");
//                udLvlReq = null;
//			}
//		}
//	}
}
