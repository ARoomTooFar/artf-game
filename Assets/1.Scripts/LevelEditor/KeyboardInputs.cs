using UnityEngine;
using System.Collections;

public class KeyboardInputs : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Backspace)) {
			MapData.removeObjects((GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController).selectedTiles);
		}

		if(Input.GetKey(KeyCode.UpArrow)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveForward();
		}

		if(Input.GetKey(KeyCode.DownArrow)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveBackward();
		}

		if(Input.GetKey(KeyCode.LeftArrow)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveLeft();
		}

		if(Input.GetKey(KeyCode.RightArrow)) {
			GameObject.Find("UICamera").GetComponent<CameraMovement>().moveRight();
		}
	}
}
