using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterTest {

	protected internal class test_ARTFRoom : ARTFRoom {
		public test_ARTFRoom(Vector3 vec1, Vector3 vec2) : base(vec1, vec2) {
		}

		public List<TerrainBlock> Blocks {
			get{ return Blocks; }
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
			foreach(List<TerrainBlock> val in dictionary.Values) {
				retVal += val.Count;
			}
			return retVal;
		}
	}

	protected internal class test_ARTFRoomManager : ARTFRoomManager {
		public test_ARTFRoomManager(){}
		public test_ARTFRoomManager(ARTFRoomManager rm) : base(rm){}

		public List<ARTFRoom> RoomList{
			get{ return base.roomList; }
		}

		public new ARTFRoom find(Vector3 pos){
			return base.find(pos);
		}
	}

}