using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyboardInputs : MonoBehaviour {	

	CameraMovement move;
	InputField mapName;

	void Start(){
		move = Camera.main.GetComponent<CameraMovement>();
		mapName = GameObject.Find("InputField_Save").GetComponent<InputField>();
	}
	// Update is called once per frame
	void Update () {
		if(mapName.isFocused) {
			return;
		}

		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			move.zoomCamIn();
		}

		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			move.zoomCamOut();
		}

		if(Input.GetKeyDown(KeyCode.Backspace)) {
			MapData.delete();
		}

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			move.moveForward();
		}

		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			move.moveBackward();
		}

		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			move.moveLeft();
		}

		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			move.moveRight();
		}

		if(Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)){
			move.zoomCamIn();
		}

		if(Input.GetKey(KeyCode.Equals)|| Input.GetKey(KeyCode.KeypadPlus)){
			move.zoomCamOut();
		}
		if(Input.GetKeyDown(KeyCode.Tab)) {
			move.toggleCamera();
		}
		if(Input.GetKeyDown(KeyCode.E)) {
			Camera.main.GetComponent<TileMapController>().fillInRoom();
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			move.changeModes();
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			move.returnToStart();
		}
	}
}
