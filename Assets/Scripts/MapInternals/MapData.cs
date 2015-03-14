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

	public static void ClearData(){
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

	public static string SaveString{
		get{
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
	public static void addRoom(Vector3 pos1, Vector3 pos2){
		TheFarRooms.add(pos1, pos2);
	}

	public static void moveRoom(Vector3 oldPos, Vector3 newPos){
		TheFarRooms.move(oldPos, newPos - oldPos);
	}

	public static void resizeRoom(Vector3 oldCor, Vector3 newCor){
		if(isResizeRoomValid(oldCor, newCor)){
			TheFarRooms.resize(oldCor, newCor);
		}
	}

	public static void removeRoom(Vector3 pos){
		TheFarRooms.remove(pos);
	}
	#endregion RoomManipulation

	#region RoomValidation
	public static bool isAddRoomValid(Vector3 pos1, Vector3 pos2){
		return TheFarRooms.doAnyRoomsIntersect(new ARTFRoom(pos1, pos2));
	}

	public static bool isMoveRoomValid(Vector3 oldPos, Vector3 newPos){
		return TheFarRooms.isMoveValid(oldPos, newPos);
	}

	public static bool isResizeRoomValid(Vector3 oldCor, Vector3 newCor){
		return TheFarRooms.isResizeValid(oldCor, newCor);
	}
	#endregion RoomValidation
	#endregion Rooms

	#region TerrainManipulation
	public static void changeTerrainType(Vector3 pos, string type){
		TerrainBlocks.changeType(pos, type);
	}

	public static void rotateTerrain(Vector3 pos, bool goClockwise = true){
		TerrainBlocks.rotate(pos, goClockwise);
	}
	#endregion TerrainManipulation

	public static void addMonsterScenery(string type, Vector3 pos, DIRECTION dir){
		GameObject obj = GameObjectResourcePool.getResource(type, pos, dir.toRotationVector());
		SceneryMonoBehaviour smb = obj.GetComponent<SceneryMonoBehaviour>();
		MonsterMonoBehaviour mmb = obj.GetComponent<MonsterMonoBehaviour>();
		GameObjectResourcePool.returnResource(type, obj);
		if(smb != null){
			MapData.addScenery(type, pos, dir);
		} 
		if(mmb != null) {
			MapData.addMonster(type, pos, dir);
		}
	}

	public static void moveMonsterScenery(GameObject obj, Vector3 pos, Vector3 offset){
		SceneryMonoBehaviour smb = obj.GetComponent<SceneryMonoBehaviour>();
		MonsterMonoBehaviour mmb = obj.GetComponent<MonsterMonoBehaviour>();
		WallCornerMonoBehaviour wmb = obj.GetComponent<WallCornerMonoBehaviour>();

		Debug.Log("mMS");
		Debug.Log(pos);

		if(smb != null){
			MapData.moveScenery(pos, offset);
		}
		if(mmb != null) {
			MapData.moveMonster(pos, offset);
		}
		if(wmb != null) {
			Debug.Log("resize");

			MapData.resizeRoom(pos, pos+offset);
		}
	}

	public static void rotateMonsterScenery(GameObject obj, Vector3 pos, bool goClockwise = true){
		if(obj.GetComponent<SceneryMonoBehaviour>() != null){
			MapData.rotateScenery(pos, goClockwise);
		} else {
			MapData.rotateMonster (pos, goClockwise);
		}
	}

	#region Monsters
	#region MonsterManipulation
	public static void addMonster(string type, Vector3 pos, DIRECTION dir){
		if(!isAddMonsterValid(pos)){
			return;
		}
		MonsterBlocks.add(new MonsterBlock(type, pos, dir));
	}

	public static void moveMonster(Vector3 pos, Vector3 offset){
		if(!isAddMonsterValid(pos + offset)){
			return;
		}
		MonsterBlocks.move(pos, offset);
	}

	public static void rotateMonster(Vector3 pos, bool goClockwise = true){
		MonsterBlocks.rotate(pos, goClockwise);
	}

	public static void removeMonster(Vector3 pos){
		MonsterBlocks.remove(pos);
	}
	#endregion MonsterManipulation

	#region MonsterValidation
	public static bool isAddMonsterValid(Vector3 pos){
		return MonsterBlocks.isAddValid(pos);
	}
	#endregion MonsterValidation
	#endregion Monsters

	#region Scenery
	#region SceneryManipulation
	public static void addScenery(string type, Vector3 pos, DIRECTION dir){
		if(!isAddSceneryValid(type, pos, dir)){
			return;
		}
		SceneryBlocks.add(new SceneryBlock(type, pos, dir));
	}

	public static void moveScenery(Vector3 pos, Vector3 offset){
		if(!isMoveSceneryValid(pos, offset)){
			return;
		}
		SceneryBlocks.move(pos, offset);
	}

	public static void rotateScenery(Vector3 pos, bool goClockwise = true){
		SceneryBlocks.rotate(pos, goClockwise);
	}

	public static void removeScenery(Vector3 pos){
		SceneryBlocks.remove(pos);
	}
	#endregion SceneryManipulation

	#region SceneryValidation
	public static bool isAddSceneryValid(string type, Vector3 pos, DIRECTION dir = DIRECTION.North){
		return SceneryBlocks.isAddValid(type, pos, dir);
	}

	public static bool isMoveSceneryValid(Vector3 pos, Vector3 offset){
		return SceneryBlocks.isMoveValid(pos, offset);
	}

	public static bool isRotateSceneryValid(Vector3 pos, bool goClockwise = true){
		return SceneryBlocks.isRotateValid(pos, goClockwise);
	}
	#endregion SceneryValidation
	#endregion Scenery
}
