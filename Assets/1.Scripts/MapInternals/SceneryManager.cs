using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryManager {

	Dictionary<string, List<SceneryBlock>> dictionary = new Dictionary<string, List<SceneryBlock>>();

	public SceneryManager() {
	}

	
	public void clear(){
		foreach(List<SceneryBlock> lst in dictionary.Values){
			foreach(SceneryBlock blk in lst){
				blk.remove();
			}
		}
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
		TerrainBlock terBlk;
		ARTFRoom rm = MapData.TheFarRooms.find (blk.Position);
		if (rm == null) {
			return false;
		}
		//for each coordinate this block occupies
		foreach(Vector3 coordinate in blk.Coordinates) {
			//get the terrain block in that position
			terBlk = MapData.TerrainBlocks.find(coordinate);
			//if there's no block then... what? continue anyways
			if(terBlk == null) {
				continue;
			}
			//if the block is linked to this piece of scenery, then unlink it
			if(terBlk.Scenery != null && terBlk.Scenery.Equals(blk)) {
				terBlk.unlinkScenery();
			}
			terBlk.addScenery(blk);
			if(!rm.inRoom(coordinate)){
				return false;
			}
			foreach(SceneryBlock scn in rm.Scenery){
				if (scn.Coordinates.Contains(coordinate)){
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
		TerrainBlock terBlk;
		//for each coordinate this block occupies
		foreach(Vector3 coordinate in blk.Coordinates) {
			//get the terrain block in that position
			terBlk = MapData.TerrainBlocks.find(coordinate);
			//if there's no block then... what? continue anyways
			if(terBlk == null) {
				continue;
			}
			//if the block is linked to this piece of scenery, then unlink it
			if(terBlk.Scenery.Equals(blk)) {
				terBlk.unlinkScenery();
			}
		}
		ARTFRoom rm = MapData.TheFarRooms.find (blk.Position);
		rm.Scenery.Remove (blk);
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
		if(blk.BlockInfo.isDoor){
			ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
			if(rm == null){
				blk.remove();
				return false;
			}
			if(!rm.isEdge(blk.Position)){
				blk.remove();
				return false;
			}
			blk.rotate(rm.getWallSide(blk.Position));
			if(!blk.Orientation.isCardinal()){
				blk.remove();
				return false;
			}
			rm.Doors.Add(blk);

			//aaron - gets wall tiles that are replaced by door
			foreach(Vector3 pos in blk.Coordinates){
				MapData.TerrainBlocks.find(pos).removeWall();
			}
		}
		//attempt to link the scenery to the appropriate terrain
		if(!linkTerrain(blk)) {
			//if something goes wrong, 
			unlinkTerrain(blk);
			return false;
		}
		//get the list for the block type
//		Debug.Log(blk.BlockID);
		List<SceneryBlock> lst;
		try{
			lst = dictionary[blk.BlockID];
		} catch {
		//create one if needed
			lst = new List<SceneryBlock>();
			dictionary.Add(blk.BlockID, lst);
		}
		//add the block to the list
		lst.Add(blk);
		if(blk.BlockInfo.isDoor) {
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
		if(blk.BlockID == "Prefabs/PlayerStartingLocation") {
			return;
		}
		if(blk.BlockID == "Prefabs/PlayerEndingLocation") {
			return;
		}
		unlinkTerrain(blk);
		if(blk.BlockInfo.isDoor) {
			ARTFRoom rm = MapData.TheFarRooms.find(blk.Position);
			if(rm != null) {
				rm.Doors.Remove(blk);
				foreach(Vector3 pos in blk.Coordinates) {
					MapData.TerrainBlocks.find(pos).addWall(rm.getWallSide(pos));
				}
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
		unlinkTerrain(blk);
		blk.rotate(goClockwise);
		linkTerrain(blk);
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

	public GameObject findGameObj(Vector3 pos) {
		//round position
		Vector3 intPosition = pos.Round();
		//for each type of block
		foreach(KeyValuePair<string, List<SceneryBlock>> kvPair in dictionary) {
			//check each block
			foreach(SceneryBlock blk in kvPair.Value) {
				//return block if position matches
				if(blk.Position.Equals(intPosition)) {
					return blk.GameObj;
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
		if(blk.BlockInfo.isDoor) {
			try{
				blk.rotate(MapData.TheFarRooms.find(pos).getWallSide(pos));
			} catch {
				blk.remove();
				return false;
			}
			if(blk.Orientation == DIRECTION.NonDirectional){
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
		ARTFRoom rm = MapData.TheFarRooms.find (blk.Position);
		if (rm == null) {
			return false;
		}
		foreach(Vector3 coor in blk.Coordinates){
			if(!rm.inRoom(coor)){
				return false;
			}
			foreach(SceneryBlock scn in rm.Scenery){
				if(scn == blk){
					continue;
				}
				if (scn.Coordinates.Contains(coor)){
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