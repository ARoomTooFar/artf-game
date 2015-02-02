using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHandler_FolderItems : MonoBehaviour {

	public Button Button_Dino; 

	public MouseHandler_TileSelection mouseControl;

	// Use this for initialization
	void Start () {
		Button_Dino = this.gameObject.GetComponent("Button") as Button;

		mouseControl = GameObject.Find("TileMap").GetComponent("MouseHandler_TileSelection") as MouseHandler_TileSelection;

		Button_Dino.onClick.AddListener (() => {
			setSelectedObject ("Prefabs/dino"); });
	}

	void setSelectedObject(string s){
		mouseControl.setSelectedObject(s);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
