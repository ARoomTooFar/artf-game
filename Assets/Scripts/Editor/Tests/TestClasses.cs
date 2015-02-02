using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterTest {

	protected internal class test_ARTFRoom : ARTFRoom {
		public test_ARTFRoom(Vector3 vec1, Vector3 vec2) : base(vec1, vec2) {
		}

		public List<TerrainBlock> Blocks {
			get{ return blocks; }
		}
	}

	protected internal class test_TerrainManager : TerrainManager {
		public test_TerrainManager(TerrainManager tm): base(tm) {
		}

		public Dictionary<string, List<TerrainBlock>> Dictionary {
			get{ return base.dictionary; }
		}

		/*
		 * public int numTiles()
		 * 
		 * Returns the number of tiles stored
		 */
		public int numTiles() {
			int retVal = 0;
			foreach(KeyValuePair<string, List<TerrainBlock>> kvp in dictionary) {
				retVal += kvp.Value.Count;
			}
			return retVal;
		}
	}

	protected internal class test_MapData : MapData{
		public static test_TerrainManager Terrain {
			get{ return new test_TerrainManager(MapData.Instance.TerrainBlocks); }
		}
	}

}