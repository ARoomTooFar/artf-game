using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class LevelPathCheck {

	public static List<Vector3> fullPath {
		get;
		private set;
	}
	public static List<SceneryBlock> roomPath {
		get;
		private set;
	}

	public static void checkPath(){
		roomPath = Pathfinder.getRoomPath(MapData.StartingRoom, MapData.EndingRoom);
		fullPath = Pathfinder.getPath(MapDataParser.start.Position, MapDataParser.end.Position);
		Debug.Log(MapDataParser.start.Position + ", " + MapDataParser.end.Position);
		Debug.Log(roomPath);
		GameObject.Find("LevelCheck").GetComponent<Text>().enabled = (fullPath == null);
	}
}
