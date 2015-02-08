using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class listeners for clicks to camera control
//buttons in the screen UI, which allow the user to switch
//to preset camera views, as well as between orthographic
//and perspective modes
public class UIHandler_CameraControls: MonoBehaviour {

//	public Button Button_TopDown = null; 
//	public Button Button_Perspective = null; 
//	public Button Button_Orthographic = null; 
//
//	//both cameras needed in order to change between perspective and orthog
//	public Camera UICamera;
//	public Camera OnTopCamera;
//
//
//
//
//	void Start () {
//		Button_TopDown.onClick.AddListener (() => {
//			changeToTopDown (); });
//		Button_Perspective.onClick.AddListener (() => {
//			changeToPerspective ();});
//		Button_Orthographic.onClick.AddListener (() => {
//			changetoOrthographic ();});
//
//	
//	}
//
//	private void changeToTopDown(){
//		UICamera.transform.rotation = Quaternion.Euler (90, 45, 0);
//	}
//
//	private void changeToPerspective(){
//		UICamera.orthographic = false;
//		OnTopCamera.orthographic = false;
//
//		//set us to a good angle
//		UICamera.transform.rotation = Quaternion.Euler (45, 45, 0);
//	}
//
//	private void changetoOrthographic(){
//		UICamera.orthographic = true;
//		OnTopCamera.orthographic = true;
//
//		//set us to a good angle
//		UICamera.transform.rotation = Quaternion.Euler (45, 45, 0);
//	}
	

}
