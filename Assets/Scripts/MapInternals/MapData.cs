using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {

	private MapData() {}

	private static MapData instance;
	private TerrainDictionary terrainBlocks = new TerrainDictionary();
	private SceneryDictionary sceneryBlocks = new SceneryDictionary();
	private MonsterDictionary monsterBlocks = new MonsterDictionary();

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

	public string getSaveString() {
		return "";
	}
}
