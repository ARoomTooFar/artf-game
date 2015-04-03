using UnityEngine;
using System.Collections;

/*
 * Controls the mode of the level editor
 * 
 */

public class Mode : MonoBehaviour {
	public bool Room = false;
	public bool Tile = true;

	public void SetRoomMode(){
		Room = true;
		Tile = false;
	}

	public void SetTileMode(){
		Room = false;
		Tile = true;
	}

}
