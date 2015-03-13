using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class Input_Camera : MonoBehaviour {

	Output_Camera cameraControl;

	static Camera UICamera;

	Vector3 oldPos;

	Button Button_TopDown;
	Button Button_Perspective;
	Button Button_Orthographic;
	Button Button_ZoomOut;
	Button Button_ZoomIn;
	Button Button_Hand; 
	Button Button_Pointer; 
	
	void Start () {
//		cameraControl = new Output_Camera();
		cameraControl = GameObject.Find("UICamera").GetComponent("Output_Camera") as Output_Camera;

		Button_TopDown = GameObject.Find ("Button_TopDown").GetComponent("Button") as Button;
		Button_Perspective = GameObject.Find ("Button_Perspective").GetComponent("Button") as Button;
		Button_Orthographic = GameObject.Find ("Button_Orthographic").GetComponent("Button") as Button;
		Button_ZoomOut = GameObject.Find ("Button_ZoomOut").GetComponent("Button") as Button;
		Button_ZoomIn = GameObject.Find ("Button_ZoomIn").GetComponent("Button") as Button;
		Button_Hand = GameObject.Find ("Button_Hand").GetComponent("Button") as Button;
		Button_Pointer = GameObject.Find ("Button_Pointer").GetComponent("Button") as Button;

		Button_ZoomIn.onClick.AddListener (() => {
			cameraControl.zoomCamIn ();});
		Button_ZoomOut.onClick.AddListener (() => {
			cameraControl.zoomCamOut ();});
		Button_TopDown.onClick.AddListener (() => {
			cameraControl.changeToTopDown (); });
		Button_Perspective.onClick.AddListener (() => {
			cameraControl.changeToPerspective ();});
		Button_Orthographic.onClick.AddListener (() => {
			cameraControl.changetoOrthographic ();});
		Button_Hand.onClick.AddListener (() => {
			cameraControl.cursorToHand (); });
		Button_Pointer.onClick.AddListener (() => {
			cameraControl.cursorToPointer ();});

		UICamera = GameObject.Find ("UICamera").camera;

		Vector3 oldPos = new Vector3();
	}

	void Update () {
		checkForMouseScrolling();
		checkForMouseClicks();

		//doesn't move cam in proper direction right now
		checkForKeyPresses();
	}

	void checkForMouseScrolling(){
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			cameraControl.zoomCamIn ();
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			cameraControl.zoomCamOut ();
		}
	}

	void checkForMouseClicks(){
		Vector3 delta = new Vector3(); 
		if (Input.GetMouseButtonDown(1)){
			oldPos = Input.mousePosition;
		}
		if (Input.GetMouseButton (1)) {
			oldPos = cameraControl.dragCamera(oldPos, delta);
		}
	}

	void checkForKeyPresses(){
		if (Input.GetKey (KeyCode.UpArrow)) {
			cameraControl.moveForward ();
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			cameraControl.moveBackward ();
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			cameraControl.moveLeft ();
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			cameraControl.moveRight ();
		}
	}
}
