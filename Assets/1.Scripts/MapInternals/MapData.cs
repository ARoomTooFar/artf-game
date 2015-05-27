using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MapData {

	static MapData() {
		SceneryBlocks = new SceneryManager();
		MonsterBlocks = new MonsterManager();
		TheFarRooms = new ARTFRoomManager();
	}

	public static void ClearData() {
		SceneryBlocks.clear();

		MonsterBlocks.clear();

		TheFarRooms.clear();
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
			//retVal += "Terrain\n";
			//retVal += TerrainBlocks.SaveString;
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
	public static bool addRoom(ARTFRoom rm){
		if(TheFarRooms.isAddValid(rm)) {
			TheFarRooms.add(rm);
		} else {
			rm.remove();
			return false;
		}
		return true;
	}

	public static bool addRoom(Vector3 pos1, Vector3 pos2) {
		return addRoom(new ARTFRoom(pos1, pos2));
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

	public static bool addObject(string type, Vector3 pos, DIRECTION dir) {
		GameObject obj = GameObjectResourcePool.getResource(type, pos, dir.toRotationVector());
		BlockData data = obj.GetComponent<BlockData>();
		GameObjectResourcePool.returnResource(type, obj);
		if(data is SceneryData) {
			if(SceneryBlocks.isAddValid(type, pos, dir)) {
				SceneryBlocks.add(new SceneryBlock(type, pos, dir));
			} else {
				return false;
			}
			LevelPathCheck.checkPath();
			return true;
		} 
		if(data is MonsterData) {
			if(MonsterBlocks.isAddValid(type, pos, dir)) {
				MonsterBlocks.add(new MonsterBlock(type, pos, dir));
			} else {
				return false;
			}
			return true;
		}
		return false;
	}

	public static void resizeRoom(GameObject obj, Vector3 pos, Vector3 offset){
		WallCornerData data = obj.GetComponent <WallCornerData>();
		
		if(data != null) {
			if(TheFarRooms.isResizeValid(pos, pos + offset)) {
				int oldCost = TheFarRooms.find(pos).Cost;
				TheFarRooms.resize(pos, pos + offset);
				int newCost = TheFarRooms.find(pos+offset).Cost;
				Money.buy(newCost - oldCost);
			}
			LevelPathCheck.checkPath();
			return;
		}
	}

	public static void dragObject(GameObject obj, Vector3 pos, Vector3 offset) {
		BlockData data = obj.GetComponent <BlockData>();

		if(data is SceneryData) {
			if(SceneryBlocks.isMoveValid(pos, offset)) {
				SceneryBlocks.move(pos, offset);
			}
			LevelPathCheck.checkPath();
			return;
		}

		if(data is MonsterData) {
			if(MonsterBlocks.isMoveValid(pos, offset)) {
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

		SceneryBlock blk;
		MonsterBlock mon;
		foreach(Vector3 vec in set) {
			blk = SceneryBlocks.find(vec);
			if(blk != null){
				obs.Add(blk.GameObj);
			}
			mon = MonsterBlocks.find(vec);
			if(mon != null){
				obs.Add(mon.GameObj);
			}
		}
		return obs;

	}

	public static void delete(){
		Debug.Log("delete");
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
