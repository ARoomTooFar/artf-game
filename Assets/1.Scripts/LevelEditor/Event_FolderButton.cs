using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Event_FolderButton : MonoBehaviour, IPointerClickHandler {
	
	//for toggling the color of the buttons
	//public bool buttonIsSelected = false;
	public FolderBarController fb;

	void Start () {
		fb = GameObject.Find ("FolderBar").GetComponent<FolderBarController>();
	}

	//When user clicks the button this script is attached to,
	//this opens the right folder, and highlights the right button
	public void OnPointerClick (PointerEventData data)
	{
		//buttonIsSelected = !buttonIsSelected; //boolean for changing button color
		fb.setFolderStates (this.name);
		fb.slideFolderBar();
		fb.setButtonStates ();
	}
}
