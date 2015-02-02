using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class listens to save/deploy buttons
public class UIHandler_FileIO : MonoBehaviour
{
	public Button Button_Save = null; // assign in the editor
	public Button Button_Deploy = null; // assign in the editor

	void Start ()
	{
		//Add listeners for various buttons
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Deploy.onClick.AddListener (() => {
			deployFile ();});

	}

	public void saveFile ()
	{

	}

	public void deployFile ()
	{

	}
		

}
