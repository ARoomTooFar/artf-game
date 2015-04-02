using UnityEngine;
using System.Collections;

public class KeyboardInputs : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Backspace)) {
			MapData.removeObjects((GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController).selectedTiles);
		}
	}
}
