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

	#region RoomManipulation
	public void addRoom(Vector3 pos1, Vector3 pos2){
		theFarRooms.addRoom(pos1, pos2);
	}

	public void moveRoom(Vector3 position, Vector3 nPosition){
		theFarRooms.moveRoom(position, nPosition - position);
	}

	public void resizeRoom(Vector3 corner, Vector3 nCorner){
		theFarRooms.resizeRoom(corner, nCorner);
	}

	public void removeRoom(Vector3 position){
		theFarRooms.removeRoom(position);
	}
	#endregion RoomManipulation

	#region TerrainManipulation
	public void changeTerrainType(Vector3 pos, string type){
		TerrainBlocks.changeType(pos, type);
	}

	public void rotateTerrain(Vector3 pos, bool goClockwise = true){
		terrainBlocks.rotate(pos, goClockwise);
	}
	#endregion TerrainManipulation

	#region MonsterManipulation
	public void addMonster(string type, Vector3 pos, DIRECTION dir){
		monsterBlocks.add(new MonsterBlock(type, pos, dir));
	}

	public void moveMonster(Vector3 pos, Vector3 offset){
		monsterBlocks.move(pos, offset);
	}

	public void removeMonster(Vector3 pos){
		monsterBlocks.remove(pos);
	}

	public void rotateMonster(Vector3 pos, bool goClockwise = true){
		monsterBlocks.rotate(pos, goClockwise);
	}
	#endregion MonsterManipulation


	#region Scenery
	#region SceneryManipulation
	public void addScenery(string type, Vector3 pos, DIRECTION dir){
		sceneryBlocks.add(new SceneryBlock(type, pos, dir));
	}

	public void moveScenery(Vector3 pos, Vector3 offset){
		sceneryBlocks.move(pos, offset);
	}

	public void removeScenery(Vector3 pos){
		SceneryBlocks.remove(pos);
	}

	public void rotateScenert(Vector3 pos, bool goClockwise = true){
		SceneryBlocks.rotate(pos, goClockwise);
	}
	#endregion SceneryManipulation

	#region SceneryValidation
	public bool isSceneryBlockValid(Vector3 position){
		return sceneryBlocks.isBlockValid(position);
	}

	public bool isSceneryRotationValid(Vector3 postion, bool goClockwise = true){
		return sceneryBlocks.isRotationValid(postion, goClockwise);
	}

	public bool isSceneryMoveValid(Vector3 position, Vector3 offset){
		return sceneryBlocks.isMoveValid(position, offset);
	}
	#endregion SceneryValidation
	#endregion Scenery
}
