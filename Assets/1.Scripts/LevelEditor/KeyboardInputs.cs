using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardInputs : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Backspace)) {

			HashSet<GameObject> obs = MapData.getObjects(Camera.main.GetComponent<TileMapController>().selectedTiles);

			//refund costs
			foreach(GameObject ob in obs){
				if(ob != null){
					Money.money += ob.GetComponent<LevelEntityData>().baseCost;
					Money.updateMoneyDisplay();
				}
			}

			MapData.removeObjects(Camera.main.GetComponent<TileMapController>().selectedTiles);
		}

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			Camera.main.GetComponent<CameraMovement>().moveForward();
		}

		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			Camera.main.GetComponent<CameraMovement>().moveBackward();
		}

		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			Camera.main.GetComponent<CameraMovement>().moveLeft();
		}

		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			Camera.main.GetComponent<CameraMovement>().moveRight();
		}

		if(Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)){
			Camera.main.GetComponent<CameraMovement>().zoomCamIn();
		}

		if(Input.GetKey(KeyCode.Equals)|| Input.GetKey(KeyCode.KeypadPlus)){
			Camera.main.GetComponent<CameraMovement>().zoomCamOut();
		}
	}
}
