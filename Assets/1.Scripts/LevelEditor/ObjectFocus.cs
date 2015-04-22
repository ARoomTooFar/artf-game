using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ObjectFocus {
	public static HashSet<GameObject> focusedObjects;

	static ObjectFocus(){
		focusedObjects = new HashSet<GameObject>();
	}

	public static void fillFocusedObjects(){
		focusedObjects = MapData.getObjects (Camera.main.GetComponent<TileMapController>().selectedTiles);
	}
	
}
