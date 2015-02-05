using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

//This class listens to save/deploy/load buttons
public class UIHandler_FileIO : MonoBehaviour
{
	public Button Button_Save = null;
	public Button Button_Deploy = null;
	public Button Button_Load = null;
	MouseHandler_TileSelection tileSelection;
	Dictionary<string, Vector3> savedState;
	DataHandler_Items data;

	void Start ()
	{

		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Load.onClick.AddListener (() => {
			loadFile ();});


		tileSelection = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
		savedState = new Dictionary<string, Vector3> ();

		data = GameObject.Find ("ItemObjects").GetComponent ("DataHandler_Items") as DataHandler_Items;

	}

	public void saveFile ()
	{
		if (data.getItemDictionary ().Count != 0) {
			savedState.Clear ();
			savedState = new Dictionary<string, Vector3> (data.getItemDictionary ());
		}else{
			Debug.Log ("Nothing to save");
		}
	}

	public void loadFile ()
	{
		if (savedState.Count != 0) {
			data.wipeItemObjects ();
			data.clearItemDictionary ();
			foreach (KeyValuePair<string, Vector3> entry in savedState) {
				string key = entry.Key.Substring (0, entry.Key.IndexOf ('_'));
				tileSelection.placeItems (key, entry.Value);
			}
		} else {
			Debug.Log ("Nothing to load");
		}
	}
		

}
