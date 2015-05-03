using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MonsterManager {

	Dictionary<string, List<MonsterBlock>> dictionary = new Dictionary<string, List<MonsterBlock>>();

	public MonsterManager() {
	}

	public void clear(){
		foreach(List<MonsterBlock> lst in dictionary.Values){
			foreach(MonsterBlock blk in lst){
				blk.remove();
			}
		}
	}

	#region Manipulation
	/*
	 * public bool addBlock (MonsterBlock block)
	 * 
	 * Adds a MonsterBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public void add(MonsterBlock blk) {
		ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
		rm.Monster.Add(blk);
		//get the list for the block type
		List<MonsterBlock> lst;
		try{
			lst = dictionary[blk.BlockInfo.BlockID];
		} catch {
			lst = new List<MonsterBlock>();
			dictionary.Add(blk.BlockInfo.BlockID, lst);
		}

		//add the block to the list
		lst.Add(blk);
	}

	#region Move
	public void move(Vector3 pos, Vector3 offset){
		move(find(pos), offset);
	}
	
	public void move(MonsterBlock blk, Vector3 offset){
		//unlinkTerrain (blk);
		blk.move(offset);
		//linkTerrain (blk);
	}
	#endregion Move

	#region Rotate
	public void rotate(Vector3 pos, bool goClockwise = true){
		rotate(find(pos), goClockwise);
	}
	
	public void rotate(MonsterBlock blk, bool goClockwise = true){
		blk.rotate(goClockwise);
	}
	#endregion Rotate

	#region Remove
	/*
	 * public bool remove (Vector3 position)
	 * 
	 * Remove a block from the map data.
	 */
	public void remove(Vector3 pos) {
		remove(find(pos));
	}
	
	public void remove(MonsterBlock blk){
		if(blk == null) {
			return;
		}
		blk.remove();
		//remove from list
		dictionary[blk.BlockInfo.BlockID].Remove(blk);
	}
	#endregion Remove
	#endregion Manipulation

	/*
	 * public TerrainBlock findBlock (Vector3 position)
	 * 
	 * Returns the block at position
	 * Returns null if there is no block in that position.
	 */
	public MonsterBlock find(Vector3 pos) {
		//for each type of block
		foreach(KeyValuePair<string, List<MonsterBlock>> kvPair in dictionary) {
			//check each block
			foreach(MonsterBlock blk in kvPair.Value) {
				//return block if position matches
				if(blk.Position.Equals(pos)) {
					return blk;
				}//otherwise continue to next
			}
		}
		//return null if none found
		return null;
	}

	#region Validation
	public bool isAddValid(string type, Vector3 pos, DIRECTION dir){
		MonsterBlock tstMon = new MonsterBlock(type, pos, dir);
		bool retVal = isMonsterValid(tstMon);
		tstMon.remove();
		return retVal;
	}

	public bool isMoveValid(Vector3 pos, Vector3 offset){
		MonsterBlock tstMon = find(pos);
		if(tstMon == null) {
			return false;
		}
		tstMon.move(offset);
		bool retVal = isMonsterValid(tstMon);
		tstMon.move(-offset);
		return retVal;
	}

	public bool isMonsterValid(MonsterBlock mon){

		ARTFRoom rm = MapData.TheFarRooms.find(mon.Position);
		if(rm == null) {
			return false;
		}
		foreach(Vector3 vec in mon.Coordinates) {
			if(!rm.inRoom(vec)){
				return false;
			}
		}
		foreach(MonsterBlock other in rm.Monster) {
			if(other == mon){
				continue;
			}
			if(mon.RadiusCoordinates.Intersect(other.RadiusCoordinates).Count() != 0){
				return false;
			}
		}
		foreach(SceneryBlock blk in rm.Scenery) {
			if(blk.Coordinates.Intersect(mon.Coordinates).Count() != 0){
				return blk.Walkable;
			}
		}
		return true;
	}
	#endregion Validation

	public string SaveString {
		get {
			string retVal = "";
			string tempVal;
			foreach(KeyValuePair<string, List<MonsterBlock>> kvPair in dictionary) {
				tempVal = "";
				tempVal += kvPair.Key + ":";
				foreach(MonsterBlock blk in kvPair.Value) {
					tempVal += blk.SaveString + " ";
				}
				retVal += tempVal + "\n";
			}
			return retVal;
		}
	}

}