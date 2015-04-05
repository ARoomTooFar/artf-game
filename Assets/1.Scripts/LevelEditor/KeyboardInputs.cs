using UnityEngine;
using System.Collections;

public class KeyboardInputs : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Backspace)) {
			MapData.removeObjects((GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController).selectedTiles);
		}

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveForward();
		}

		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveBackward();
		}

		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveLeft();
		}

		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveRight();
		}

		if(Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)){
			GameObject.Find("UICamera").GetComponent<CameraMovement>().zoomCamIn();
		}

		if(Input.GetKey(KeyCode.Equals)|| Input.GetKey(KeyCode.KeypadPlus)){
			GameObject.Find("UICamera").GetComponent<CameraMovement>().zoomCamOut();
		}
	}
}
