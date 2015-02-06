using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryManager {

	Dictionary<string, List<SceneryBlock>> dictionary = new Dictionary<string, List<SceneryBlock>>();

	public SceneryManager() {
	}

	/*
	 * public bool addBlock (SceneryBlock block)
	 * 
	 * Adds a SceneryBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public bool addBlock(SceneryBlock block) {
		//attempt to link the scenery to the appropriate terrain
		if(!linkTerrain(block)) {
			//if something goes wrong, 
			unlinkTerrain(block);
			return false;
		}
		//get the list for the block type
		List<SceneryBlock> lst = dictionary[block.BlockInfo.BlockID];
		//create one if needed
		if(lst == null) {
			lst = new List<SceneryBlock>();
			dictionary.Add(block.BlockInfo.BlockID, lst);
		}
		//add the block to the list
		lst.Add(block);

		return true;
	}

	/*
	 * private bool linkTerrain (SceneryBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * 
	 * Returns true if successful.
	 * Returns false if a piece of terrain is already linked to scenery
	 */
	private bool linkTerrain(SceneryBlock block) {
		TerrainBlock blk;
		//for each coordinate this block occupies
		foreach(Vector3 coordinate in block.Coordinates) {
			//get the terrain block in that position
			blk = MapData.Instance.TerrainBlocks.findBlock(coordinate);
			//if there's no block there, this block is not placeable
			if(blk == null) {
				return false;
			}
			//try to link the scenery, return false if problem
			if(!blk.addScenery(block)) {
				return false;
			}
		}
		return true;
	}

	/*
	 * private void unlinkTerrain(SceneryBlock block)
	 * 
	 * Break the link to this SceneryBlock from associated terrain blocks
	 * 
	 */
	private void unlinkTerrain(SceneryBlock block) {
		TerrainBlock blk;
		//for each coordinate this block occupies
		foreach(Vector3 coordinate in block.Coordinates) {
			//get the terrain block in that position
			blk = MapData.Instance.TerrainBlocks.findBlock(coordinate);
			//if there's no block then... what? continue anyways
			if(blk == null) {
				continue;
			}
			//if the block is linked to this piece of scenery, then unlink it
			if(blk.Scenery.Equals(block)) {
				blk.removeScenery();
			}
		}
	}

	/*
	 * public bool removeBlock (Vector3 position)
	 * 
	 * Remove a piece of scenery from the map data.
	 * 
	 * returns true if the scenery wasn't or is no longer part of the data
	 * returns false if something bad happens
	 */
	public bool removeBlock(Vector3 position) {
		//round position
		Vector3 intPosition = position.Round();
		//find block at position
		SceneryBlock tgtBlock = findBlock(intPosition);
		if(tgtBlock == null) {
			//if block doesn't exist, return true
			return true;
		}
		//unlink terrain
		unlinkTerrain(tgtBlock);
		//remove from list
		return dictionary[tgtBlock.BlockInfo.BlockID].Remove(tgtBlock);
	}

	/*
	 * public SceneryBlock findBlock (Vector3 position)
	 * 
	 * Returns the scenery at position
	 * Returns null if there is no block in that position.
	 */
	public SceneryBlock findBlock(Vector3 position) {
		//round position
		Vector3 intPosition = position.Round();
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

	public void moveBlock(SceneryBlock blk, Vector3 offset) {
		unlinkTerrain(blk);
		blk.move(offset);
		linkTerrain(blk);
	}

	public void moveBlock(Vector3 pos, Vector3 offset) {
		moveBlock(findBlock(pos), offset);
	}

	public void rotateBlock(SceneryBlock blk, DIRECTION dir) {
		unlinkTerrain(blk);
		blk.Orientation = dir;
		linkTerrain(blk);
	}

	public void rotateBlock(Vector3 pos, DIRECTION dir) {
		rotateBlock(findBlock(pos), dir);
	}

	public bool isRotationValid(Vector3 pos, DIRECTION dir) {
		return isRotationValid(findBlock(pos), dir);
	}

	public bool isRotationValid(SceneryBlock blk, DIRECTION dir) {
		DIRECTION oDir = blk.Orientation;
		blk.Orientation = dir;
		bool retVal = isBlockValid(blk);
		blk.Orientation = oDir;
		return retVal;
	}

	public bool isMoveValid(Vector3 pos, Vector3 offset) {
		return isMoveValid(findBlock(pos), offset);
	}

	public bool isMoveValid(SceneryBlock blk, Vector3 offset) {
		blk.move(offset);
		bool retVal = isBlockValid(blk);
		blk.move(-offset);
		return retVal;
	}

	public bool isBlockValid(Vector3 pos) {
		return isBlockValid(findBlock(pos));
	}

	public bool isBlockValid(SceneryBlock blk) {
		TerrainBlock terBlk;
		foreach(Vector3 coordinate in blk.Coordinates) {
			//get the terrain block in that position
			terBlk = MapData.Instance.TerrainBlocks.findBlock(coordinate);
			//if there's no block there, this block is not placeable
			if(terBlk == null) {
				return false;
			}
			//try to link the scenery, return false if problem
			if(terBlk != null) {
				return false;
			}
			if(terBlk.Scenery != blk) {
				return false;
			}
		}
		return true;
	}
	
	public string ScenerySaveString {
		get {
			string retVal = "";
			string tempVal;
			foreach(KeyValuePair<string, List<SceneryBlock>> kvPair in dictionary) {
				tempVal = "";
				tempVal += kvPair.Key + ": ";
				foreach(SceneryBlock blk in kvPair.Value) {
					tempVal += blk.SaveString + " ";
				}
				retVal += tempVal + "\n";
			}
			return retVal;
		}
	}

}