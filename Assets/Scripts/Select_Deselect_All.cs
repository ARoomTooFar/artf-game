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

		//Button initialization
		Button_Select_All = GameObject.Find ("Button_Select_All").GetComponent ("Button") as Button;
		Button_Deselect_All = GameObject.Find ("Button_Deselect_All").GetComponent ("Button") as Button;
		Button_Select_All_Only = GameObject.Find ("Button_Select_All_Only").GetComponent ("Button") as Button;

		//Script initialization
		select_Deselect_All = this.gameObject.GetComponent ("Select_Deselect_All") as Select_Deselect_All;

		//Listener initialization
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
		//for each child in children...
		foreach (Transform child in children) {
			//if the current component is NOT the script, continue
			if(!child.gameObject.GetComponent ("Output_ItemObjectUI")) {
				continue;
			}
			//Otherwise, execute the toggle
			else{
				child.gameObject.GetComponent<ItemObjectUI>().toggleItemObjectUI();
			}
		}
	

	}

	//selects all, regardless of current selection
	public void selectAll() {

		GameObject itemObjects = this.gameObject;
		Transform[] children = itemObjects.GetComponentsInChildren<Transform> ();
		//for each child in children...
		foreach (Transform child in children) {
			//if the current component is NOT the script, continue
			if(!child.gameObject.GetComponent ("Output_ItemObjectUI")) {
				continue;
			}
			//otherwise...
			else{
				//if the object is not already selected
				if(!child.gameObject.GetComponent<ItemObjectUI>().toggleItemObjectUI()){
					//select it
					child.gameObject.GetComponent<ItemObjectUI>().toggleItemObjectUI();
				}
			}
		}
	}

	//deselects all, regardless of current selection
	public void deselectAll() {
		GameObject itemObjects = this.gameObject;
		Transform[] children = itemObjects.GetComponentsInChildren<Transform> ();
		//for each child in children...
		foreach (Transform child in children) {
			//if the current component is NOT the script, continue
			if(!child.gameObject.GetComponent ("Output_ItemObjectUI")) {
				continue;
			}
			//otherwise...
			else{
				//if the object is selected
				if(child.gameObject.GetComponent<ItemObjectUI>().toggleItemObjectUI()){
					//deselect it
					child.gameObject.GetComponent<ItemObjectUI>().toggleItemObjectUI();
				}
			}
		}
	}
}
