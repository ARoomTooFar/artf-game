using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class listens to the editing controls on the left hand side
//of the screen (zoom in, zoom out, rotate, hand, and pointer)
public class UIHandler_EditingControls : MonoBehaviour {

	public Button Button_Hand = null; 
	public Button Button_Pointer = null; 
	public Button Button_Rotate = null;
	public Button Button_ZoomOut = null; 
	public Button Button_ZoomIn = null; 

	public TransformHandler_Camera transformHandler_Camera;

	// Use this for initialization
	void Start () {

		Button_Hand.onClick.AddListener (() => {
			cursorToHand (); });
		Button_Pointer.onClick.AddListener (() => {
			cursorToPointer ();});
		Button_Rotate.onClick.AddListener (() => {
			rotateObject ();});
		Button_ZoomIn.onClick.AddListener (() => {
			zoomIn ();});
		Button_ZoomOut.onClick.AddListener (() => {
			zoomOut ();});

	
	}

	private void cursorToHand(){

	}

	private void cursorToPointer(){
		
	}

	private void rotateObject(){
		//implement when we have object-focus functionality
	}

	private void zoomIn(){
		transformHandler_Camera.zoomCamIn(2f);
	}

	private void zoomOut(){
		transformHandler_Camera.zoomCamOut(2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
