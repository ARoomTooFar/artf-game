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
	public GameObject tilemap; //reference to the tile map to suppress selection

	public TransformHandler_Camera UICamera;

	// Use this for initialization
	void Start () {

		tilemap = GameObject.Find ("TileMap");
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
		suppressSelection ();
		UICamera.zoomCamIn(2f);
	}

	private void zoomOut(){
		UICamera.zoomCamOut(2f);
	}

	//If a button is clicked, don't select the tile below it, if there is a tile
	private void suppressSelection() {

		Vector3 mousePos = Input.mousePosition;
		tilemap.GetComponent <MouseHandler_TileSelection> ().deselect (mousePos);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
