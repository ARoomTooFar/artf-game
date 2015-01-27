using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {

	private MapData() {}

	private static MapData instance;
	private TerrainDictionary terrainBlocks = new TerrainDictionary();
	private SceneryDictionary sceneryBlocks = new SceneryDictionary();
	private MonsterDictionary monsterBlocks = new MonsterDictionary();
	private ARTFRoomManager theFarRooms = new ARTFRoomManager();

	public static MapData Instance {
		get {
			if(instance == null) {
				instance = new MapData();
			}
			return instance;
		}
	}

	public TerrainDictionary TerrainBlocks {
		get { return terrainBlocks; }
	}

	public SceneryDictionary SceneryBlocks {
		get{ return sceneryBlocks; }
	}

	public MonsterDictionary MonsterBlocks {
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
		theFarRooms.moveRoom(position, nPosition.Subtract(position));
	}
	#endregion RoomManipulation
}
