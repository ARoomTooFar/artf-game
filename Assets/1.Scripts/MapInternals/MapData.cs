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

	public static ARTFTerminalRoom StartingRoom;// = new ARTFTerminalRoom(new Vector3(0, 0, 0), new Vector3(7, 0, 7));

	public static ARTFTerminalRoom EndingRoom;// = new ARTFTerminalRoom(new Vector3(0, 0, 8), new Vector3(7, 0, 15));

	public static string SaveString {
		get {
			string retVal = "MapData\n";
			retVal += "Terrain\n";
			retVal += TerrainBlocks.SaveString;
			retVal += "Terminal\n";
			retVal += StartingRoom.SaveString + " " + EndingRoom.SaveString + "\n";
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
	public static void addRoom(Vector3 pos1, Vector3 pos2) {
		if(TheFarRooms.isAddValid(pos1, pos2)) {
			TheFarRooms.add(pos1, pos2);
		}
	}

	public static void moveRoom(Vector3 oldPos, Vector3 newPos) {
		if(TheFarRooms.isMoveValid(oldPos, newPos)){
			TheFarRooms.move(oldPos, newPos - oldPos);
		}
		LevelPathCheck.checkPath();
	}

	public static void removeRoom(Vector3 pos) {
		TheFarRooms.remove(pos);
		LevelPathCheck.checkPath();
	}
	#endregion Rooms

	public static void addObject(string type, Vector3 pos, DIRECTION dir) {
		GameObject obj = GameObjectResourcePool.getResource(type, pos, dir.toRotationVector());
		BlockData data = obj.GetComponent<BlockData>();
		GameObjectResourcePool.returnResource(type, obj);
		if(data is SceneryData) {
			if(SceneryBlocks.isAddValid(type, pos, dir)) {
				SceneryBlocks.add(new SceneryBlock(type, pos, dir));
			}
			LevelPathCheck.checkPath();
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
			LevelPathCheck.checkPath();
			return;
		}

		if(data is SceneryData) {
			if(SceneryBlocks.isMoveValid(pos, offset)) {
				SceneryBlocks.move(pos, offset);
			}
			LevelPathCheck.checkPath();
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
			LevelPathCheck.checkPath();
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
		LevelPathCheck.checkPath();
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

	public static void delete(){
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
	
}
