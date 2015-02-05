using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

//This class listens to save/deploy buttons
public class UIHandler_FileIO : MonoBehaviour
{
	public Button Button_Save = null; // assign in the editor
	public Button Button_Deploy = null; // assign in the editor

	MouseHandler_TileSelection tileSelection;
	public Dictionary<string, Vector3> savedState;

	void Start ()
	{
		//Add listeners for various buttons
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Deploy.onClick.AddListener (() => {
			deployFile ();});


		tileSelection = GameObject.Find("TileMap").GetComponent("MouseHandler_TileSelection") as MouseHandler_TileSelection;
		savedState = new Dictionary<string, Vector3>();

	}

	public void saveFile ()
	{
		savedState.Clear ();
		savedState = new Dictionary<string, Vector3>(tileSelection.placedItems);
	}

	public void deployFile ()
	{

		//should be in public functions in mousehandler_tileselection
		foreach(Transform child in tileSelection.sceneObjects){
			GameObject.Destroy(child.gameObject);
		}
		tileSelection.placedItems.Clear ();


		tileSelection.placedItems = new Dictionary<string, Vector3>(savedState);
		Debug.Log (savedState.Count + ", " + tileSelection.placedItems.Count);


		foreach(KeyValuePair<string, Vector3> entry in savedState)
		{
			string key = entry.Key.Substring(0, entry.Key.IndexOf ('_'));
//			Debug.Log(key + ", " + entry.Value);
			tileSelection.placeItems(key, entry.Value);
		}
		


//		for(int i = 0; i < savedState.Count; i++){
//			tileSelection.placeItems(savedState[i], savedState[i].);
//		}

//		if(savedState.Count != 0){
//			for(int i = 0; i < savedState.Count; i++){
//				tileSelection.placedItems.Add(savedState[i]);
//			}
//		}
	}
		

}
