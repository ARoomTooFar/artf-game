using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class LevelPathCheck {

	private static PathfinderDebugDraw pdd = null;

	public static List<SceneryBlock> roomPath { get; private set; }

	public static void checkPath() {
		foreach(ARTFRoom rm in MapData.TheFarRooms.roomList) {
			rm.linkRoomsViaDoors();
		}
		roomPath = Pathfinder.getRoomPath(MapData.StartingRoom, MapData.EndingRoom);
		//fullPath = Pathfinder.getPath(MapDataParser.start.Position, MapDataParser.end.Position);
		GameObject obj = GameObject.Find("LevelCheck");
		if(obj != null) {
			obj.GetComponent<Text>().enabled = (roomPath == null);
		}
		if(Global.inLevelEditor){
		if(pdd == null) {
			pdd = Camera.main.GetComponent<PathfinderDebugDraw>();
		}
		pdd.forceUpdate = true;
		}
	}
}
