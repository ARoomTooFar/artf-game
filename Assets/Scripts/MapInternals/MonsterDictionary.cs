using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryDictionary : Dictionary<string, List<SceneryBlock>> {
	public SceneryDictionary() {
	}

	/*
	 * public bool addBlock (SceneryBlock block)
	 * 
	 * Adds a SceneryBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public bool addBlock(SceneryBlock block) {
		//attempt to link the input to its neighbors
		if(!linkTerrain(block)) {
			//if something goes wrong, 
			unlinkTerrain(block);
			return false;
		}
		//get the list for the block type
		List<SceneryBlock> lst = this[block.BlockInfo.BlockID];
		//create one if needed
		if(lst == null) {
			lst = new List<SceneryBlock>();
			this.Add(block.BlockInfo.BlockID, lst);
		}
		//add the block to the list
		lst.Add(block);

		return true;
	}

	/*
	 * private bool linkTerrain (SceneryBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * Also links block as a neighbor to any adjacent blocks.
	 * 
	 * Returns true if successful.
	 * Returns false if a block already has a neighbor in that position.
	 */
	private bool linkTerrain(SceneryBlock block) {
		TerrainBlock blk;
		foreach(Vector3 coordinate in block.Coordinates) {
			blk = MapData.Instance.TerrainBlocks.findBlock(block.Position);
			if(blk == null){
				return false;
			}
			if(!blk.addScenery(block)){
				return false;
			}
		}
		return true;
	}

	/*
	 * private void unlinkNeighbors(TerrainBlock block)
	 * 
	 * Breaks all neighbor links between block and its list of neighbors.
	 * 
	 */
	private void unlinkTerrain(SceneryBlock block) {
		TerrainBlock blk;
		foreach(Vector3 coordinate in block.Coordinates) {
			blk = MapData.Instance.TerrainBlocks.findBlock(block.Position);
			if(blk == null){
				continue;
			}
			if(blk.Scenery.Equals(block)){
				blk.removeScenery();
			}
		}
	}

	/*
	 * public bool removeBlock (Vector3 position)
	 * 
	 * Remove a block from the map data.
	 * 
	 * returns true if the block wasn't or is no longer part of the data
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
		//unlink neighbors
		unlinkTerrain(tgtBlock);
		//remove from list
		return this[tgtBlock.BlockInfo.BlockID].Remove(tgtBlock);
	}

	/*
	 * public TerrainBlock findBlock (Vector3 position)
	 * 
	 * Returns the block at position
	 * Returns null if there is no block in that position.
	 */
	public SceneryBlock findBlock(Vector3 position) {
		//round position
		Vector3 intPosition = position.Round();
		//for each type of block
		foreach(KeyValuePair<string, List<SceneryBlock>> kvPair in this) {
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

	public string ScenerySaveString {
		get {
			string retVal = "";
			string tempVal;
			foreach(KeyValuePair<string, List<SceneryBlock>> kvPair in this) {
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