using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ObjectFocus {
	public static HashSet<GameObject> focusedObjects;

	static ObjectFocus(){
		focusedObjects = new HashSet<GameObject>();
	}

	public static void fillFocusedObjects(){
		focusedObjects = MapData.getObjects ((GameObject.Find ("TileMap").GetComponent ("TileMapController") as TileMapController).selectedTiles);
	}
	
}
