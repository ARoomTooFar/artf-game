using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Select_Deselect_All : MonoBehaviour {

	Select_Deselect_All select_Deselect_All;
	Button Button_Select_All; 
	Button Button_Deselect_All;
	Button Button_Select_All_Only;
	// Use this for initialization
	void Start () {
		Button_Select_All = GameObject.Find ("Button_Select_All").GetComponent ("Button") as Button;
		Button_Deselect_All = GameObject.Find ("Button_Deselect_All").GetComponent ("Button") as Button;
		Button_Select_All_Only = GameObject.Find ("Button_Select_All_Only").GetComponent ("Button") as Button;
		select_Deselect_All = this.gameObject.GetComponent ("Select_Deselect_All") as Select_Deselect_All;
		Button_Select_All.onClick.AddListener (() => {
			select_Deselect_All.selectOrDeselectAll (); 
		});
		Button_Select_All_Only.onClick.AddListener (() => {
			select_Deselect_All.selectAll (); 
		});
		Button_Deselect_All.onClick.AddListener (() => {
			select_Deselect_All.deselectAll (); 
		});

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//selects or deselects all objects in field
	public void selectOrDeselectAll() {

		GameObject itemObjects = this.gameObject;
		Transform[] children = itemObjects.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			if(!child.gameObject.GetComponent ("Output_ItemObjectUI")) {
				continue;
			}
			else{
				child.gameObject.GetComponent<Output_ItemObjectUI>().toggleItemObjectUI();
			}
		}
	

	}

	public void selectAll() {

		GameObject itemObjects = this.gameObject;
		Transform[] children = itemObjects.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			if(!child.gameObject.GetComponent ("Output_ItemObjectUI")) {
				continue;
			}
			else{
				if(!child.gameObject.GetComponent<Output_ItemObjectUI>().toggleItemObjectUI()){
					child.gameObject.GetComponent<Output_ItemObjectUI>().toggleItemObjectUI();
				}
			}
		}
	}

	public void deselectAll() {
		GameObject itemObjects = this.gameObject;
		Transform[] children = itemObjects.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children) {
			if(!child.gameObject.GetComponent ("Output_ItemObjectUI")) {
				continue;
			}
			else{
				if(child.gameObject.GetComponent<Output_ItemObjectUI>().toggleItemObjectUI()){
					child.gameObject.GetComponent<Output_ItemObjectUI>().toggleItemObjectUI();
				}
			}
		}
	}
}
