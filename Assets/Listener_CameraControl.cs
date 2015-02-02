using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Listener_CameraControl : MonoBehaviour {

	public Button Button_TopDown = null; // assign in the editor
	public Button Button_Perspective = null; // assign in the editor
	public Button Button_Orthographic = null; // assign in the editor

	//both cameras needed in order to change between perspective and orthog
	public Camera UICamera;
	public Camera OnTopCamera;


	void Start () {
		Button_TopDown.onClick.AddListener (() => {
			changeToTopDown (); });
		Button_Perspective.onClick.AddListener (() => {
			changeToPerspective ();});
		Button_Orthographic.onClick.AddListener (() => {
			changetoOrthographic ();});

	
	}

	private void changeToTopDown(){
		UICamera.transform.rotation = Quaternion.Euler (90, 0, 0);
	}

	private void changeToPerspective(){
		UICamera.orthographic = false;
		OnTopCamera.orthographic = false;

		//set us to a good angle
		UICamera.transform.rotation = Quaternion.Euler (45, 45, 0);
	}

	private void changetoOrthographic(){
		UICamera.orthographic = true;
		OnTopCamera.orthographic = true;

		//set us to a good angle
		UICamera.transform.rotation = Quaternion.Euler (45, 45, 0);
	}
	

}
