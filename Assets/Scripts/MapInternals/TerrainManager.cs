using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager {

	protected internal Dictionary<string, List<TerrainBlock>> dictionary =
		new Dictionary<string, List<TerrainBlock>>();

	//constructor
	public TerrainManager() {
	}

	//Shallow copy constructor. Only for testing
	protected internal TerrainManager(TerrainManager other){
		dictionary = other.dictionary;
	}

	#region (un)linkNeighbors
	/*
	 * private bool linkNeighbors (TerrainBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * Also links block as a neighbor to any adjacent blocks.
	 * 
	 * Returns true if successful.
	 * Returns false if a block already has a neighbor in that position.
	 */
	private bool linkNeighbors(TerrainBlock blk) {
		//Go through every set of blocks
		foreach(List<TerrainBlock> lst in dictionary.Values) {
			//for each extant block
			foreach(TerrainBlock other in lst) {
				//determine if the block is a neighbor of the input
				DIRECTION dir = blk.isNeighbor(other);
				//if not, move on to the next one
				if(dir == DIRECTION.NonDirectional) {
					continue;
				}
				//set the found block as a neighbor of the input
				blk.addNeighbor(other, dir);
				//try to set the input as a neighbor of the found block.
				//if something goes wrong, stop the whole function.
				if(!other.addNeighbor(blk, dir.Opposite())) {
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
	private void unlinkNeighbors(TerrainBlock blk) {
		//for each TerrainBlock in block's list of neighbors
		foreach(KeyValuePair<DIRECTION, TerrainBlock> other in blk.Neighbors) {
			//remove block from the neighbors list of neighbors
			other.Value.removeNeighbor(other.Key.Opposite());
		}
		//remove all neighbors from block;
		blk.clearNeighbors();
	}

	public bool relinkNeighbors(TerrainBlock blk) {
		unlinkNeighbors(blk);
		return linkNeighbors(blk);
	}
	#endregion (un)linkNeighbors

	#region Manipulation
	/*
	 * public bool addBlock (TerrainBlock block)
	 * 
	 * Adds a TerrainBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public bool add(TerrainBlock blk) {
		//attempt to link the input to its neighbors
		if(!linkNeighbors(blk)) {
			//if something goes wrong, 
			unlinkNeighbors(blk);
			return false;
		}
		//get the list for the block type
		List<TerrainBlock> lst;
		try{
			lst = dictionary[blk.BlockInfo.BlockID];
		} catch (Exception){
			//create one if needed
			lst = new List<TerrainBlock>();
			dictionary.Add(blk.BlockInfo.BlockID, lst);
		}
		
		
		//add the block to the list
		lst.Add(blk);
		
		return true;
	}

	#region Remove
	/*
	 * public bool removeBlock (Vector3 position)
	 * 
	 * Remove a block from the map data.
	 * 
	 * returns true if the block wasn't or is no longer part of the data
	 * returns false if something bad happens
	 */
	public bool remove(Vector3 pos) {
		return remove(find(pos));
	}

	public bool remove(TerrainBlock blk){
		//unlink neighbors
		unlinkNeighbors(blk);
		MapData.Instance.SceneryBlocks.remove(blk.Scenery);
		MapData.Instance.MonsterBlocks.remove(blk.Monster);
		//remove from list
		return dictionary[blk.BlockInfo.BlockID].Remove(blk);
	}
	#endregion Remove


	#region Rotate
	public void rotate(Vector3 pos, bool goClockwise = true){
		rotate(find(pos), goClockwise);
	}

	public void rotate(TerrainBlock blk, bool goClockwise = true){
		blk.rotate(goClockwise);
	}
	#endregion Rotate

	#region changeType
	public void changeType(Vector3 pos, string type){
		changeType(find(pos), type);
	}

	public void changeType(TerrainBlock blk, string type){
		blk.changeType(type);
	}
	#endregion changeType
	#endregion Manipulation

	
	/*
	 * public TerrainBlock findBlock (Vector3 position)
	 * 
	 * Returns the block at position
	 * Returns null if there is no block in that position.
	 */
	public TerrainBlock find(Vector3 pos) {
		//round position
		Vector3 intPosition = pos.Round();
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
}