using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardInputs : MonoBehaviour {	

	CameraMovement move;

	void Start(){
		move = Camera.main.GetComponent<CameraMovement>();
	}
	// Update is called once per frame
	void Update () {
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
	}
}
