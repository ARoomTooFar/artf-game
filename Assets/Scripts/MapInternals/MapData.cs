using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {

	#region PrivateVariables
	protected static MapData instance;
	#endregion PrivateVariables

	protected MapData() {
		TerrainBlocks = new TerrainManager();
		SceneryBlocks = new SceneryManager();
		MonsterBlocks = new MonsterManager();
		TheFarRooms = new ARTFRoomManager();
	}

	public static MapData Instance {
		get {
			if(instance == null) {
				instance = new MapData();
			}
			return instance;
		}
	}

	public static void ClearData(){
		instance = null;
	}

	public TerrainManager TerrainBlocks {
		get;
		private set;
	}

	public SceneryManager SceneryBlocks {
		get;
		private set;
	}

	public MonsterManager MonsterBlocks {
		get;
		private set;
	}

	public ARTFRoomManager TheFarRooms {
		get;
		private set;
	}

	public string getSaveString() {
		return "";
	}

	#region Rooms
	#region RoomManipulation
	public void addRoom(Vector3 pos1, Vector3 pos2){
		TheFarRooms.add(pos1, pos2);
	}

	public void moveRoom(Vector3 oldPos, Vector3 newPos){
		TheFarRooms.move(oldPos, newPos - oldPos);
	}

	public void resizeRoom(Vector3 oldCor, Vector3 newCor){
		TheFarRooms.resize(oldCor, newCor);
	}

	public void removeRoom(Vector3 pos){
		TheFarRooms.remove(pos);
	}
	#endregion RoomManipulation

	#region RoomValidation
	public bool isAddRoomValid(Vector3 pos1, Vector3 pos2){
		return TheFarRooms.doAnyRoomsIntersect(new ARTFRoom(pos1, pos2));
	}

	public bool isMoveRoomValid(Vector3 oldPos, Vector3 newPos){
		return TheFarRooms.isMoveValid(oldPos, newPos);
	}

	public bool isResizeRoomValid(Vector3 oldCor, Vector3 newCor){
		return TheFarRooms.isResizeValid(oldCor, newCor);
	}
	#endregion RoomValidation
	#endregion Rooms

	#region TerrainManipulation
	public void changeTerrainType(Vector3 pos, string type){
		TerrainBlocks.changeType(pos, type);
	}

	public void rotateTerrain(Vector3 pos, bool goClockwise = true){
		TerrainBlocks.rotate(pos, goClockwise);
	}
	#endregion TerrainManipulation

	#region Monsters
	#region MonsterManipulation
	public void addMonster(string type, Vector3 pos, DIRECTION dir){
		MonsterBlocks.add(new MonsterBlock(type, pos, dir));
	}

	public void moveMonster(Vector3 pos, Vector3 offset){
		MonsterBlocks.move(pos, offset);
	}

	public void rotateMonster(Vector3 pos, bool goClockwise = true){
		MonsterBlocks.rotate(pos, goClockwise);
	}

	public void removeMonster(Vector3 pos){
		MonsterBlocks.remove(pos);
	}
	#endregion MonsterManipulation

	#region MonsterValidation
	public bool isAddMonsterValid(Vector3 pos){
		return MonsterBlocks.isAddValid(pos);
	}
	#endregion MonsterValidation
	#endregion Monsters

	#region Scenery
	#region SceneryManipulation
	public void addScenery(string type, Vector3 pos, DIRECTION dir){
		SceneryBlocks.add(new SceneryBlock(type, pos, dir));
	}

	public void moveScenery(Vector3 pos, Vector3 offset){
		SceneryBlocks.move(pos, offset);
	}

	public void rotateScenery(Vector3 pos, bool goClockwise = true){
		SceneryBlocks.rotate(pos, goClockwise);
	}

	public void removeScenery(Vector3 pos){
		SceneryBlocks.remove(pos);
	}
	#endregion SceneryManipulation

	#region SceneryValidation
	public bool isAddSceneryValid(string type, Vector3 pos, DIRECTION dir = DIRECTION.North){
		return isAddSceneryValid(type, pos, dir);
	}

	public bool isMoveSceneryValid(Vector3 pos, Vector3 offset){
		return SceneryBlocks.isMoveValid(pos, offset);
	}

	public bool isRotateSceneryValid(Vector3 pos, bool goClockwise = true){
		return SceneryBlocks.isRotateValid(pos, goClockwise);
	}
	#endregion SceneryValidation
	#endregion Scenery
}
