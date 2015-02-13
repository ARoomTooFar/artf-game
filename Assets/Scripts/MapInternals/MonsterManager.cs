using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager {

	Dictionary<string, List<MonsterBlock>> dictionary = new Dictionary<string, List<MonsterBlock>>();

	public MonsterManager() {
	}

	/*
	 * public bool addBlock (MonsterBlock block)
	 * 
	 * Adds a MonsterBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public bool add(MonsterBlock block) {
		//attempt to link the input to its neighbors
		if(!linkTerrain(block)) {
			//if something goes wrong, 
			unlinkTerrain(block);
			return false;
		}
		//get the list for the block type
		List<MonsterBlock> lst = dictionary[block.BlockInfo.BlockID];
		//create one if needed
		if(lst == null) {
			lst = new List<MonsterBlock>();
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
	 * Also links block as a neighbor to any adjacent blocks.
	 * 
	 * Returns true if successful.
	 * Returns false if a block already has a neighbor in that position.
	 */
	private bool linkTerrain(MonsterBlock block) {
		TerrainBlock blk = MapData.Instance.TerrainBlocks.find(block.Position);
		if(blk == null) {
			return false;
		}
		if(!blk.addMonster(block)) {
			return false;
		}

		return true;
	}

	/*
	 * private void unlinkNeighbors(TerrainBlock block)
	 * 
	 * Breaks all neighbor links between block and its list of neighbors.
	 * 
	 */
	private void unlinkTerrain(MonsterBlock block) {
		TerrainBlock blk = MapData.Instance.TerrainBlocks.find(block.Position);
		if(blk.Monster.Equals(block)){
			blk.removeMonster();
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
	public bool remove(Vector3 position) {
		//round position
		Vector3 intPosition = position.Round();
		//find block at position
		MonsterBlock tgtBlock = find(intPosition);
		if(tgtBlock == null) {
			//if block doesn't exist, return true
			return true;
		}
		//unlink neighbors
		unlinkTerrain(tgtBlock);
		//remove from list
		return dictionary[tgtBlock.BlockInfo.BlockID].Remove(tgtBlock);
	}

	/*
	 * public TerrainBlock findBlock (Vector3 position)
	 * 
	 * Returns the block at position
	 * Returns null if there is no block in that position.
	 */
	public MonsterBlock find(Vector3 position) {
		//round position
		Vector3 intPosition = position.Round();
		//for each type of block
		foreach(KeyValuePair<string, List<MonsterBlock>> kvPair in dictionary) {
			//check each block
			foreach(MonsterBlock blk in kvPair.Value) {
				//return block if position matches
				if(blk.Position.Equals(intPosition)) {
					return blk;
				}//otherwise continue to next
			}
		}
		//return null if none found
		return null;
	}

	public void move(Vector3 pos, Vector3 offset){
		find(pos).move(offset);
	}

	public void rotate(MonsterBlock blk, bool goClockwise = true){
		blk.rotate(goClockwise);
	}

	public void rotate(Vector3 pos, bool goClockwise = true){
		rotate(pos, goClockwise);
	}

	public string SaveString {
		get {
			string retVal = "";
			string tempVal;
			foreach(KeyValuePair<string, List<MonsterBlock>> kvPair in dictionary) {
				tempVal = "";
				tempVal += kvPair.Key + ": ";
				foreach(MonsterBlock blk in kvPair.Value) {
					tempVal += blk.SaveString + " ";
				}
				retVal += tempVal + "\n";
			}
			return retVal;
		}
	}

}