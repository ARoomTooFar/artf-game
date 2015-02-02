using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Listener_FolderItems : MonoBehaviour {

	public Button Button_Dino; 

	public MouseControl mouseControl;

	// Use this for initialization
	void Start () {
		Button_Dino = this.gameObject.GetComponent("Button") as Button;

		mouseControl = GameObject.Find("TileMap").GetComponent("MouseControl") as MouseControl;

		Button_Dino.onClick.AddListener (() => {
			setSelectedObject ("dino"); });
	}

	void setSelectedObject(string s){
		mouseControl.setSelectedObject(s);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
