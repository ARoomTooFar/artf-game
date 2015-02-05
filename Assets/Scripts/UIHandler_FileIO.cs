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
	public static Dictionary<string, Vector3> savedState;

	void Start ()
	{
		//Add listeners for various buttons
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Deploy.onClick.AddListener (() => {
			deployFile ();});


		tileSelection = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
		savedState = new Dictionary<string, Vector3> ();

	}

	public void saveFile ()
	{
		Debug.Log ("Saved state was length: " + savedState.Count);
		savedState.Clear ();
		savedState = new Dictionary<string, Vector3> (tileSelection.getPlacedItems ());
		Debug.Log ("Saved is now length: " + savedState.Count);
	}

	public void deployFile ()
	{

		tileSelection.wipeSceneObjects ();
		tileSelection.clearPlacedItems ();
		foreach (KeyValuePair<string, Vector3> entry in savedState) {
			string key = entry.Key.Substring (0, entry.Key.IndexOf ('_'));
			//			Debug.Log(key + ", " + entry.Value);
			tileSelection.placeItems (key, entry.Value);
		}

//		Debug.Log ("placedItems size: " + tileSelection.getPlacedItems().Count);
//		tileSelection.setPlacedItems (savedState);
//		Debug.Log ("New placedItems size: " + tileSelection.getPlacedItems ().Count);
//
//		tileSelection.clearPlacedItems ();

		


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
