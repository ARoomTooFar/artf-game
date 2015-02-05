using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHandler_FolderItems : MonoBehaviour
{

//	Button Button_Dino; 

	public GameObject itemList;
	public Button[] buttonList;
	public int numberOfButtons;
	MouseHandler_TileSelection mouseControl;

	// Use this for initialization
	void Start ()
	{
		//get number of items in scrollview and make an array for them
		numberOfButtons = itemList.gameObject.transform.GetChildCount ();
		buttonList = new Button[numberOfButtons];
		
		//add all the items to their array
		for (int i = 0; i < numberOfButtons; i++) {
			buttonList [i] = itemList.gameObject.transform.GetChild (i).gameObject.GetComponent ("Button") as Button;
		}

//		Button_Dino = this.gameObject.GetComponent("Button") as Button;

		mouseControl = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;

		for (int i = 0; i < numberOfButtons; i++) {

			//have to construct this string outside the button listener declaration
			//below, or it will return a strange looking IndexArrayOutOfBoundsException
			string g = "Prefabs/" + buttonList [i].transform.gameObject.name;

			buttonList [i].onClick.AddListener (() => {
				setSelectedObject (g); });
		}

//		Button_Dino.onClick.AddListener (() => {
//			setSelectedObject ("Prefabs/dino"); });
	}

	void setSelectedObject (string s)
	{
		mouseControl.setSelectedObject (s);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
