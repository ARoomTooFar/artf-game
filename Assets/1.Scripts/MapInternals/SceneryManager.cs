using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryManager {

	Dictionary<string, List<SceneryBlock>> dictionary = new Dictionary<string, List<SceneryBlock>>();

	public SceneryManager() {
	}
	
	public void clear() {
		foreach(List<SceneryBlock> lst in dictionary.Values) {
			foreach(SceneryBlock blk in lst) {
				//make the block clean itself up
				blk.remove();
				//remove it from any rooms it's in
				ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
				if(rm != null) {
					rm.Scenery.Remove(blk);
				}
			}
		}
		dictionary.Clear();
	}

	#region (un)linkTerrain
	/*
	 * private bool linkTerrain (SceneryBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * 
	 * Returns true if successful.
	 * Returns false if a piece of terrain is already linked to scenery
	 */
	private bool linkTerrain(SceneryBlock blk) {
		ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
		if(rm == null) {
			return false;
		}
		//for each coordinate this block occupies
		foreach(Vector3 coordinate in blk.Coordinates) {
			if(!rm.inRoom(coordinate)) {
				return false;
			}
			foreach(SceneryBlock scn in rm.Scenery) {
				if(scn.Coordinates.Contains(coordinate)) {
					return false;
				}
			}
		}
		rm.Scenery.Add(blk);
		return true;
	}
	
	/*
	 * private void unlinkTerrain(SceneryBlock block)
	 * 
	 * Break the link to this SceneryBlock from associated terrain blocks
	 * 
	 */
	private void unlinkTerrain(SceneryBlock blk) {
		ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
		if(rm == null) {
			return;
		}
		rm.Scenery.Remove(blk);
	}
	#endregion (un)linkTerrain

	#region Manipulation
	/*
	 * public bool add (SceneryBlock block)
	 * 
	 * Adds a SceneryBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public bool add(SceneryBlock blk) {
		ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
		if(blk.SceneryBlockInfo.isDoor) {
			blk.rotate(rm.getWallSide(blk.Position));
			rm.Doors.Add(blk);
			rm.setStretchWalls(blk.Orientation);
		}

		rm.Scenery.Add(blk);
		//get the list for the block type
		List<SceneryBlock> lst;
		try {
			lst = dictionary[blk.BlockID];
		} catch {
			//create one if needed
			lst = new List<SceneryBlock>();
			dictionary.Add(blk.BlockID, lst);
		}
		//add the block to the list
		lst.Add(blk);
		if(blk.SceneryBlockInfo.isDoor) {
			if(isAddValid(blk.BlockID, blk.doorCheckPosition, blk.Orientation.Opposite())) {
				add(new SceneryBlock(blk.BlockID, blk.doorCheckPosition, blk.Orientation.Opposite()));
			}
		}

		return true;
	}

	#region Remove
	/*
	 * public bool remove (Vector3 position)
	 * 
	 * Remove a piece of scenery from the map data.
	 * 
	 * returns true if the scenery wasn't or is no longer part of the data
	 * returns false if something bad happens
	 */
	public void remove(Vector3 pos) {
		remove(find(pos));
	}
	
	public void remove(SceneryBlock blk) {
		if(blk == null) {
			return;
		}
		if(blk.BlockID == "LevelEditor/Other/PlayerStartingLocation") {
			return;
		}
		if(blk.BlockID == "LevelEditor/Other/PlayerEndingLocation") {
			return;
		}
		unlinkTerrain(blk);
		if(blk.SceneryBlockInfo.isDoor) {
			ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
			if(rm != null) {
				rm.Doors.Remove(blk);
				rm.setStretchWalls(blk.Orientation);
			}
		}
		blk.remove();
		dictionary[blk.BlockID].Remove(blk);
	}
	#endregion Remove


	#region Move
	public void move(Vector3 pos, Vector3 offset) {
		move(find(pos), offset);
	}

	public void move(SceneryBlock blk, Vector3 offset) {
		unlinkTerrain(blk);
		blk.move(offset);
		linkTerrain(blk);
	}
	#endregion Move

	#region Rotate
	public void rotate(Vector3 pos, bool goClockwise = true) {
		rotate(find(pos), goClockwise);
	}

	public void rotate(SceneryBlock blk, bool goClockwise = true) {
		blk.rotate(goClockwise);
	}
	#endregion Rotate
	#endregion Manipulation

	
	/*
	 * public SceneryBlock find (Vector3 position)
	 * 
	 * Returns the scenery at position
	 * Returns null if there is no block in that position.
	 */
	public SceneryBlock find(Vector3 pos) {
		//round position
		Vector3 intPosition = pos.Round();
		//for each type of block
		foreach(KeyValuePair<string, List<SceneryBlock>> kvPair in dictionary) {
			//check each block
			foreach(SceneryBlock blk in kvPair.Value) {
				//return block if position matches
				if(blk.Position.Equals(intPosition)) {
					return blk;
				}//otherwise continue to next
			}
		}
		//return null if none found
		return null;
	}

	#region Validation
	#region isRotateValid
	public bool isRotateValid(Vector3 pos, bool goClockwise = true) {
		return isRotateValid(find(pos), goClockwise);
	}

	public bool isRotateValid(SceneryBlock blk, bool goClockwise = true) {
		blk.rotate(goClockwise);
		bool retVal = isBlockValid(blk);
		blk.rotate(!goClockwise);
		return retVal;
	}
	#endregion isRotateValid

	#region isMoveValid
	public bool isMoveValid(Vector3 pos, Vector3 offset) {
		return isMoveValid(find(pos), offset);
	}

	public bool isMoveValid(SceneryBlock blk, Vector3 offset) {
		blk.move(offset);
		bool retVal = isBlockValid(blk);
		blk.move(-offset);
		return retVal;
	}
	#endregion isMoveValid

	public bool isAddValid(string type, Vector3 pos, DIRECTION dir = DIRECTION.North) {
		SceneryBlock blk = new SceneryBlock(type, pos, dir);
		if(blk.SceneryBlockInfo.isDoor) {
			try {
				blk.rotate(MapData.TheFarRooms.find(pos).getWallSide(pos));
			} catch {
				blk.remove();
				return false;
			}
			if(blk.Orientation == DIRECTION.NonDirectional) {
				blk.remove();
				return false;
			}
		}
		bool retVal = isBlockValid(blk);
		blk.remove();
		return retVal;
	}

	#region isBlockValid
	public bool isBlockValid(Vector3 pos) {
		return isBlockValid(find(pos));
	}

	public bool isBlockValid(SceneryBlock blk) {
		ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
		if(rm == null) {
			return false;
		}

		if(blk.SceneryBlockInfo.isDoor) {
			if(!rm.isEdge(blk.Position)) {
				return false;
			}
			blk.rotate(rm.getWallSide(blk.Position));
			if(!blk.Orientation.isCardinal()) {
				return false;
			}
			foreach(Vector3 vec in blk.Coordinates){
				if(rm.isCorner(vec)){
					return false;
				}
			}
		}

		if(rm == MapData.StartingRoom || rm == MapData.EndingRoom) {
			if(blk.BlockID != "LevelEditor/Other/PlayerStartingLocation" &&
				blk.BlockID != "LevelEditor/Other/PlayerEndingLocation" &&
			   blk.BlockID != "{0}/Rooms/doortile"){
				return false;
			}
		}
		foreach(Vector3 coor in blk.Coordinates) {
			if(!rm.inRoom(coor)) {
				return false;
			}
			foreach(SceneryBlock scn in rm.Scenery) {
				if(scn == blk) {
					continue;
				}
				if(scn.Coordinates.Contains(coor)) {
					return false;
				}
			}
			foreach(MonsterBlock mon in rm.Monster){
				if(mon.Coordinates.Contains(coor)){
					return false;
				}
			}
		}
		return true;


	}
	#endregion isBlockValid
	#endregion Validation
	
	public string SaveString {
		get {
			string retVal = "";
			string tempVal;
			foreach(KeyValuePair<string, List<SceneryBlock>> kvPair in dictionary) {
				tempVal = "";
				tempVal += kvPair.Key + ":";
				foreach(SceneryBlock blk in kvPair.Value) {
					tempVal += blk.SaveString + " ";
				}
				retVal += tempVal + "\n";
			}
			return retVal;
		}
	}

}