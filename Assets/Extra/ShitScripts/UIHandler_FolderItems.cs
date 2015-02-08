using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHandler_FolderItems : MonoBehaviour
{
//
////	Button Button_Dino; 
//
//	public GameObject itemList;
//	public Button[] buttonList;
//	public int numberOfButtons;
////	MouseHandler_TileSelection mouseControl;
////	MouseHandler_DraggingItemFromFolder mouseHandler_DraggingItemFromFolder;
//
//	// Use this for initialization
//	void Start ()
//	{
//
////		mouseControl = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
////		mouseHandler_DraggingItemFromFolder = 
////			GameObject.Find ("MouseHandler_DraggingItemFromFolder")
////				.GetComponent ("MouseHandler_DraggingItemFromFolder") as MouseHandler_DraggingItemFromFolder;
//
//
//
//		//get number of items in scrollview and make an array for them
//		numberOfButtons = itemList.gameObject.transform.GetChildCount ();
//		buttonList = new Button[numberOfButtons];
//		
//		//add all the items to their array
//		for (int i = 0; i < numberOfButtons; i++) {
//			buttonList [i] = itemList.gameObject.transform.GetChild (i).gameObject.GetComponent ("Button") as Button;
//		}
//
//		for (int i = 0; i < numberOfButtons; i++) {
//
//			//have to construct this string outside the button listener declaration
//			//below, or it will return a strange looking IndexArrayOutOfBoundsException
////			string g = "Prefabs/" + buttonList [i].transform.gameObject.name;
////			Image u = buttonList[i].image;
////
////			buttonList [i]. onClick.AddListener (() => {
////				setSelectedObject (g, u); });
//		}
//	}
//
//	void setSelectedObject (string s, Image i)
//	{
//		Debug.Log(i);
////		mouseHandler_DraggingItemFromFolder.dragItem(s, i);
////		mouseControl.setSelectedObject (s);
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//
//	}
}
