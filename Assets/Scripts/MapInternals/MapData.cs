using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {

	#region PrivateVariables
	protected MapData() {}

	protected static MapData instance;
	protected TerrainManager terrainBlocks = new TerrainManager();
	protected SceneryManager sceneryBlocks = new SceneryManager();
	protected MonsterManager monsterBlocks = new MonsterManager();
	protected ARTFRoomManager theFarRooms = new ARTFRoomManager();
	#endregion PrivateVariables

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
		get { return terrainBlocks; }
	}

	public SceneryManager SceneryBlocks {
		get{ return sceneryBlocks; }
	}

	public MonsterManager MonsterBlocks {
		get{ return monsterBlocks; }
	}

	public ARTFRoomManager TheRooms{
		get { return theFarRooms; }
	}

	public string getSaveString() {
		return "";
	}

	#region Rooms
	#region RoomManipulation
	public void addRoom(Vector3 pos1, Vector3 pos2){
		theFarRooms.add(pos1, pos2);
	}

	public void moveRoom(Vector3 oldPos, Vector3 newPos){
		theFarRooms.move(oldPos, newPos - oldPos);
	}

	public void resizeRoom(Vector3 oldCor, Vector3 newCor){
		theFarRooms.resize(oldCor, newCor);
	}

	public void removeRoom(Vector3 pos){
		theFarRooms.remove(pos);
	}
	#endregion RoomManipulation

	#region RoomValidation
	public bool isAddRoomValid(Vector3 pos1, Vector3 pos2){
		return theFarRooms.doAnyRoomsIntersect(new ARTFRoom(pos1, pos2));
	}

	public bool isMoveRoomValid(Vector3 oldPos, Vector3 newPos){
		return theFarRooms.isMoveValid(oldPos, newPos);
	}

	public bool isResizeRoomValid(Vector3 oldCor, Vector3 newCor){
		return theFarRooms.isResizeValid(oldCor, newCor);
	}
	#endregion RoomValidation
	#endregion Rooms

	#region TerrainManipulation
	public void changeTerrainType(Vector3 pos, string type){
		TerrainBlocks.changeType(pos, type);
	}

	public void rotateTerrain(Vector3 pos, bool goClockwise = true){
		terrainBlocks.rotate(pos, goClockwise);
	}
	#endregion TerrainManipulation

	#region Monsters
	#region MonsterManipulation
	public void addMonster(string type, Vector3 pos, DIRECTION dir){
		monsterBlocks.add(new MonsterBlock(type, pos, dir));
	}

	public void moveMonster(Vector3 pos, Vector3 offset){
		monsterBlocks.move(pos, offset);
	}

	public void rotateMonster(Vector3 pos, bool goClockwise = true){
		monsterBlocks.rotate(pos, goClockwise);
	}

	public void removeMonster(Vector3 pos){
		monsterBlocks.remove(pos);
	}
	#endregion MonsterManipulation

	#region MonsterValidation
	public bool isAddMonsterValid(Vector3 pos){
		return monsterBlocks.isAddValid(pos);
	}
	#endregion MonsterValidation
	#endregion Monsters

	#region Scenery
	#region SceneryManipulation
	public void addScenery(string type, Vector3 pos, DIRECTION dir){
		sceneryBlocks.add(new SceneryBlock(type, pos, dir));
	}

	public void moveScenery(Vector3 pos, Vector3 offset){
		sceneryBlocks.move(pos, offset);
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
		return sceneryBlocks.isMoveValid(pos, offset);
	}

	public bool isRotateSceneryValid(Vector3 pos, bool goClockwise = true){
		return sceneryBlocks.isRotateValid(pos, goClockwise);
	}
	#endregion SceneryValidation
	#endregion Scenery
}
