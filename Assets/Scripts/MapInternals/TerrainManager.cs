using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager {

	Dictionary<string, List<TerrainBlock>> dictionary = new Dictionary<string, List<TerrainBlock>>();
	public TerrainManager() {
	}

	/*
	 * public bool addBlock (TerrainBlock block)
	 * 
	 * Adds a TerrainBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public bool addBlock(TerrainBlock block) {
		//attempt to link the input to its neighbors
		if(!linkNeighbors(block)) {
			//if something goes wrong, 
			unlinkNeighbors(block);
			return false;
		}
		//get the list for the block type
		List<TerrainBlock> lst;
		try{
			lst = dictionary[block.BlockInfo.BlockID];
		} catch (Exception){
			//create one if needed
			lst = new List<TerrainBlock>();
			dictionary.Add(block.BlockInfo.BlockID, lst);
		}


		//add the block to the list
		lst.Add(block);

		return true;
	}

	/*
	 * private bool linkNeighbors (TerrainBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * Also links block as a neighbor to any adjacent blocks.
	 * 
	 * Returns true if successful.
	 * Returns false if a block already has a neighbor in that position.
	 */
	private bool linkNeighbors(TerrainBlock block) {
		//Go through every set of blocks
		foreach(KeyValuePair<string, List<TerrainBlock>> pair in dictionary) {
			//for each extant block
			foreach(TerrainBlock blk in pair.Value) {
				//determine if the block is a neighbor of the input
				DIRECTION dir = block.isNeighbor(blk);
				//if not, move on to the next one
				if(dir == DIRECTION.NonDirectional) {
					continue;
				}
				//set the found block as a neighbor of the input
				block.addNeighbor(blk, dir);
				//try to set the input as a neighbor of the found block.
				//if something goes wrong, stop the whole function.
				if(!blk.addNeighbor(block, dir.Opposite())) {
					return false;
				}
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
	private void unlinkNeighbors(TerrainBlock block) {
		//for each TerrainBlock in block's list of neighbors
		foreach(KeyValuePair<DIRECTION, TerrainBlock> blk in block.Neighbors) {
			//remove block from the neighbors list of neighbors
			blk.Value.removeNeighbor(blk.Key.Opposite());
		}
		//remove all neighbors from block;
		block.clearNeighbors();
	}

	public bool relinkNeighbors(TerrainBlock block) {
		unlinkNeighbors(block);
		return linkNeighbors(block);
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
		TerrainBlock tgtBlock = findBlock(intPosition);
		if(tgtBlock == null) {
			//if block doesn't exist, return true
			return true;
		}
		//unlink neighbors
		unlinkNeighbors(tgtBlock);
		MapData.Instance.SceneryBlocks.removeBlock(position);
		MapData.Instance.MonsterBlocks.removeBlock(position);
		//remove from list
		return dictionary[tgtBlock.BlockInfo.BlockID].Remove(tgtBlock);
	}

	/*
	 * public TerrainBlock findBlock (Vector3 position)
	 * 
	 * Returns the block at position
	 * Returns null if there is no block in that position.
	 */
	public TerrainBlock findBlock(Vector3 position) {
		//round position
		Vector3 intPosition = position.Round();
		//for each type of block
		foreach(KeyValuePair<string, List<TerrainBlock>> kvPair in dictionary) {
			//check each block
			foreach(TerrainBlock blk in kvPair.Value) {
				//return block if position matches
				if(blk.Position.Equals(intPosition)) {
					return blk;
				}//otherwise continue to next
			}
		}
		//return null if none found
		return null;
	}

	public string TerrainSaveString {
		get {
			string retVal = "";
			string tempVal;
			foreach(KeyValuePair<string, List<TerrainBlock>> kvPair in dictionary) {
				tempVal = "";
				tempVal += kvPair.Key + ": ";
				foreach(TerrainBlock blk in kvPair.Value) {
					tempVal += blk.SaveString + " ";
				}
				retVal += tempVal + "\n";
			}
			return retVal;
		}
	}

	/*
	 * public int numTiles()
	 * 
	 * Returns the number of tiles stored
	 */
	public int numTiles(){
		int retVal = 0;
		foreach(KeyValuePair<string, List<TerrainBlock>> kvp in dictionary) {
			retVal += kvp.Value.Count;
		}
		return retVal;
	}
}