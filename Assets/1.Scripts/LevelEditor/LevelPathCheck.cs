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
		foreach(ARTFRoom rm in MapData.TheFarRooms.roomList) {
			rm.linkRoomsViaDoors();
		}
		roomPath = Pathfinder.getRoomPath(MapData.StartingRoom, MapData.EndingRoom);
		fullPath = Pathfinder.getPath(MapDataParser.start.Position, MapDataParser.end.Position);
		GameObject obj = GameObject.Find("LevelCheck");
		if(obj != null) {
			obj.GetComponent<Text>().enabled = (fullPath == null);
		}
	}
}
