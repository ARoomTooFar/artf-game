using System;
using UnityEngine;

public static class MapDataParser {
	public static SceneryBlock start;
	public static SceneryBlock end;

	public static void ParseSaveString(string SaveString) {
		MapData.ClearData();
		string[] SaveStringLines = SaveString.Split('\n');
		int i = 0;
		while(SaveStringLines[i] != "Terrain") {
			i++;
		}
		i++;
		while(SaveStringLines[i] != "Terminal") {
			parseTerrain(SaveStringLines[i++]);
		}
		i++;
		while(SaveStringLines[i] != "Room") {
			parseTerminalRooms(SaveStringLines[i++]);
		}
		i++;
		while(SaveStringLines[i] != "Scenery") {
			parseRoom(SaveStringLines[i++]);
		}
		i++;
		while(SaveStringLines[i] != "Monster") {
			parseScenery(SaveStringLines[i++]);
		}
		i++;
		while(i < SaveStringLines.Length-1) {
			parseMonster(SaveStringLines[i++]);
		}
		if(Global.inLevelEditor) {
			Mode.setTileMode();
		} else {
            Debug.Log(Resources.Load("Player1"));

			GameObject p1 = GameObject.Instantiate(Resources.Load("Player1"), start.Coordinates[0], Quaternion.identity) as GameObject;
            GameObject p2 = GameObject.Instantiate(Resources.Load("Player2"), start.Coordinates[1], Quaternion.identity) as GameObject;
            GameObject p3 = GameObject.Instantiate(Resources.Load("Player3"), start.Coordinates[2], Quaternion.identity) as GameObject;
            GameObject p4 = GameObject.Instantiate(Resources.Load("Player4"), start.Coordinates[3], Quaternion.identity) as GameObject;

            Loadgear loadgear = GameObject.Find("/Loadgear").GetComponent<Loadgear>();
            loadgear.players[0] = p1.GetComponent<Character>();
            loadgear.players[1] = p2.GetComponent<Character>();
            loadgear.players[2] = p3.GetComponent<Character>();
            loadgear.players[3] = p4.GetComponent<Character>();
		}
		LevelPathCheck.checkPath();
	}

	private static void parseRoom(string SaveString) {
		string[] type = SaveString.Split(':');
		string[] rooms = type[1].Trim().Split(' ');
		foreach(string rm in rooms) {
			string[] rmParams = rm.Split(',');
			if(rmParams.Length != 6){
				continue;
			}

			Vector3 pos1 = new Vector3(float.Parse(rmParams[0]),
			                           float.Parse(rmParams[1]),
			                           float.Parse(rmParams[2]));
			Vector3 pos2 = new Vector3(float.Parse(rmParams[3]),
			                         float.Parse(rmParams[4]),
			                         float.Parse(rmParams[5]));
			MapData.addRoom(pos1, pos2);           
		}
	}

	private static void parseTerrain(string SaveString) {
        //Debug.Log(SaveString);
		string[] type = SaveString.Split(':');
		string[] blocks = type[1].Trim().Split(' ');
		foreach(string blk in blocks) {
			string[] blkParams = blk.Split(',');
			if(blkParams.Length != 5){
				continue;
			}
            // Debug.Log(blkParams[0] + ", " + blkParams[1] + ", " + blkParams[2] + ": " + type[0]);
			Vector3 pos = new Vector3(float.Parse(blkParams[0]),
			                          float.Parse(blkParams[1]),
			                          float.Parse(blkParams[2]));
			TerrainBlock nBlk = new TerrainBlock(type[0], pos, (DIRECTION)Enum.Parse(typeof(DIRECTION), blkParams[3]));
			nBlk.wallType = blkParams[4];
			MapData.TerrainBlocks.add(nBlk);
		}
	}

	private static void parseScenery(string SaveString) {
		string[] type = SaveString.Split(':');
		string[] blocks = type[1].Trim().Split(' ');
		foreach(string blk in blocks) {
			string[] blkParams = blk.Split(',');
			if(blkParams.Length != 4){
				continue;
			}
			Vector3 pos = new Vector3(float.Parse(blkParams[0]),
			                          float.Parse(blkParams[1]),
			                          float.Parse(blkParams[2]));
			SceneryBlock nBlk = null;
			if(MapData.SceneryBlocks.isAddValid(type[0], pos, (DIRECTION)Enum.Parse(typeof(DIRECTION), blkParams[3]))){
				nBlk = new SceneryBlock(type[0], pos, (DIRECTION)Enum.Parse(typeof(DIRECTION), blkParams[3]));
				MapData.SceneryBlocks.add(nBlk);
			}
			if(type[0] == "LevelEditor/Other/PlayerStartingLocation") {
				start = nBlk;
			}
			if(type[0] == "LevelEditor/Other/PlayerEndingLocation") {
				end = nBlk;
			}
		}
	}

	private static void parseMonster(string SaveString) {
		string[] type = SaveString.Split(':');
		string[] blocks = type[1].Trim().Split(' ');
		foreach(string blk in blocks) {
			string[] blkParams = blk.Split(',');
			if(blkParams.Length != 5){
				continue;
			}
			Vector3 pos = new Vector3(float.Parse(blkParams[0]),
			                          float.Parse(blkParams[1]),
			                          float.Parse(blkParams[2]));
			MonsterBlock nBlk = new MonsterBlock(type[0], pos, (DIRECTION)Enum.Parse(typeof(DIRECTION), blkParams[3]));
			nBlk.Tier = Convert.ToInt32(blkParams[4]);
			MapData.MonsterBlocks.add(nBlk);
		}
	}

	private static void parseTerminalRooms(string SaveString) {
		string[] rms = SaveString.Split(' ');
		string[] rmParams = rms[0].Split(',');
		Vector3 pos1 = new Vector3(float.Parse(rmParams[0]),
			                           float.Parse(rmParams[1]),
			                           float.Parse(rmParams[2]));
		Vector3 pos2 = new Vector3(float.Parse(rmParams[3]),
			                           float.Parse(rmParams[4]),
			                           float.Parse(rmParams[5]));
		ARTFTerminalRoom rm = new ARTFTerminalRoom(pos1, pos2);
		MapData.StartingRoom = rm;
		MapData.TheFarRooms.add(rm);

		rmParams = rms[1].Split(',');
		pos1 = new Vector3(float.Parse(rmParams[0]),
		                           float.Parse(rmParams[1]),
		                           float.Parse(rmParams[2]));
		pos2 = new Vector3(float.Parse(rmParams[3]),
		                           float.Parse(rmParams[4]),
		                           float.Parse(rmParams[5]));
		rm = new ARTFTerminalRoom(pos1, pos2);
		MapData.EndingRoom = rm;
		MapData.TheFarRooms.add(rm);
	}
}