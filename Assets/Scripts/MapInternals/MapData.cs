using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData {

	private MapData() {
	}

	private static MapData instance;

	public static MapData Instance {
		get {
			if(instance == null) {
				instance = new MapData();
			}
			return instance;
		}
	}

	private TerrainDictionary terrainBlocks = new TerrainDictionary();

	public TerrainDictionary TerrainBlocks {
		get {
			return terrainBlocks;
		}
	}

	private SceneryDictionary sceneryBlocks = new SceneryDictionary();

	public SceneryDictionary SceneryBlocks {
		get{ return sceneryBlocks; }
	}

	public string getSaveString() {
		return "";
	}
}
