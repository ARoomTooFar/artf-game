using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class is for listening to buttons and UI elements that are on the canvas
//attached to the camera, which comprise the "screen UI"
public class Listener_ScreenUI : MonoBehaviour
{
		[SerializeField]
		private Button
				Button_Open = null; // assign in the editor
	
		[SerializeField]
		private Button
				Button_Save = null; // assign in the editor

		void Start ()
		{
				//Add listeners for various buttons
				Button_Open.onClick.AddListener (() => {
						openFile (); });
				Button_Save.onClick.AddListener (() => {
						saveFile ();});
		}

		public void saveFile ()
		{

		}

		public void openFile ()
		{

		}
		

}
