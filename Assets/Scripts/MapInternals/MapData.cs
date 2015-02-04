using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {

	protected MapData() {}

	protected static MapData instance;
	protected TerrainManager terrainBlocks = new TerrainManager();
	protected SceneryManager sceneryBlocks = new SceneryManager();
	protected MonsterManager monsterBlocks = new MonsterManager();
	protected ARTFRoomManager theFarRooms = new ARTFRoomManager();

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
	public bool replaceTerrain(Vector3 pos, string type){
		TerrainBlock blk = TerrainBlocks.findBlock(pos);
		if(blk == null) {
			throw new UnityException("No TerrainBlock at provided location");
		}
		return blk.changeType(type);
	}
	#endregion TerrainManipulation

	#region SceneryManipulation
	public void addScenery(string type, Vector3 pos, DIRECTION dir){
		sceneryBlocks.addBlock(new SceneryBlock(type, pos, dir);
	}
	#endregion SceneryManipulation
}
