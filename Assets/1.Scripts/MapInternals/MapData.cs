using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MapData {

	static MapData() {
		TerrainBlocks = new TerrainManager();
		SceneryBlocks = new SceneryManager();
		MonsterBlocks = new MonsterManager();
		TheFarRooms = new ARTFRoomManager();
	}

	public static void ClearData() {
		SceneryBlocks.clear();
		MonsterBlocks.clear();
		TerrainBlocks.clear();
		TheFarRooms.clear();
	}

	public static TerrainManager TerrainBlocks {
		get;
		private set;
	}

	public static SceneryManager SceneryBlocks {
		get;
		private set;
	}

	public static MonsterManager MonsterBlocks {
		get;
		private set;
	}

	public static ARTFRoomManager TheFarRooms {
		get;
		private set;
	}

	public static string SaveString {
		get {
			string retVal = "MapData\n";
			retVal += "Terrain\n";
			retVal += TerrainBlocks.SaveString;
			retVal += "Room\n";
			retVal += TheFarRooms.SaveString;
			retVal += "Scenery\n";
			retVal += SceneryBlocks.SaveString;
			retVal += "Monster\n";
			retVal += MonsterBlocks.SaveString;
			return retVal;
		}
	}

	#region Rooms
	#region RoomManipulation
	public static void addRoom(Vector3 pos1, Vector3 pos2) {
		if(TheFarRooms.isAddValid(pos1, pos2)) {
			TheFarRooms.add(pos1, pos2, false, false);
		}
	}

	public static bool addStartRoom(Vector3 pos1, Vector3 pos2) {
		if(TheFarRooms.isAddValid(pos1, pos2) && TheFarRooms.isStartOrEndRoomValid(pos1, pos2)) {
			TheFarRooms.add(pos1, pos2, true, false);
			return true;
		}else{
			return false;
		}
	}

	public static bool addEndRoom(Vector3 pos1, Vector3 pos2) {
		if(TheFarRooms.isAddValid(pos1, pos2) && TheFarRooms.isStartOrEndRoomValid(pos1, pos2)) {
			TheFarRooms.add(pos1, pos2, false, true);
			return true;
		}else{
			return false;
		}
	}

	public static void moveRoom(Vector3 oldPos, Vector3 newPos) {
		TheFarRooms.move(oldPos, newPos - oldPos);
	}

	public static void removeRoom(Vector3 pos) {
		TheFarRooms.remove(pos);
	}
	#endregion RoomManipulation

	public static bool isMoveRoomValid(Vector3 oldPos, Vector3 newPos) {
		return TheFarRooms.isMoveValid(oldPos, newPos);
	}
	#endregion Rooms

	#region TerrainManipulation
	public static void changeTerrainType(Vector3 pos, string type) {
		TerrainBlocks.changeType(pos, type);
	}

	public static void rotateTerrain(Vector3 pos, bool goClockwise = true) {
		TerrainBlocks.rotate(pos, goClockwise);
	}
	#endregion TerrainManipulation

	public static void addObject(string type, Vector3 pos, DIRECTION dir) {
		GameObject obj = GameObjectResourcePool.getResource(type, pos, dir.toRotationVector());
		BlockData data = obj.GetComponent<BlockData>();
		GameObjectResourcePool.returnResource(type, obj);
		if(data is SceneryData) {
			if(SceneryBlocks.isAddValid(type, pos, dir)) {
				SceneryBlocks.add(new SceneryBlock(type, pos, dir));
			}
			return;
		} 
		if(data is MonsterData) {
			if(MonsterBlocks.isAddValid(pos)) {
				MonsterBlocks.add(new MonsterBlock(type, pos, dir));
			}
			return;
		}
	}

	public static void dragObject(GameObject obj, Vector3 pos, Vector3 offset) {
		BlockData data = obj.GetComponent<BlockData>();

		if(data is WallCornerData) {
			if(TheFarRooms.isResizeValid(pos, pos + offset)) {
				TheFarRooms.resize(pos, pos + offset);
			}
			return;
		}

		if(data is SceneryData) {
			if(SceneryBlocks.isMoveValid(pos, offset)) {
				SceneryBlocks.move(pos, offset);
			}
			return;
		}

		if(data is MonsterData) {
			if(MonsterBlocks.isAddValid(pos+offset)) {
				MonsterBlocks.move(pos, offset);
			}
			return;
		}

	}

	public static void rotateObject(GameObject obj, Vector3 pos, bool goClockwise = true) {
		BlockData data = obj.GetComponent<BlockData>();

		if(data is SceneryData) {
			if(SceneryBlocks.isRotateValid(pos, goClockwise)) {
				SceneryBlocks.rotate(pos, goClockwise);
			}
			return;
		}

		if(data is MonsterData) {
			MonsterBlocks.rotate(pos, goClockwise);
			return;
		}
	}

	public static void removeObjects(HashSet<Vector3> set){
		foreach(Vector3 vec in set) {
			removeObject(vec);
		}
	}

	public static void removeObject(Vector3 pos) {
		SceneryBlocks.remove(pos);
		MonsterBlocks.remove(pos);
	}

	public static HashSet<GameObject> getObjects(HashSet<Vector3> set){
		HashSet<GameObject> obs = new HashSet<GameObject>();

		foreach(Vector3 vec in set) {
			obs.Add(SceneryBlocks.findGameObj(vec));
			obs.Add(MonsterBlocks.findGameObj(vec));
		}
		return obs;

	}
	
}
