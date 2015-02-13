using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager {

	Dictionary<string, List<MonsterBlock>> dictionary = new Dictionary<string, List<MonsterBlock>>();

	public MonsterManager() {
	}

	#region Manipulation
	/*
	 * public bool addBlock (MonsterBlock block)
	 * 
	 * Adds a MonsterBlock to the appropriate list.
	 * Returns true if successful.
	 * Returns false if a block already seems to exist in its position.
	 */
	public void add(MonsterBlock block) {
		//attempt to link the input to its neighbors
		linkTerrain(block);
		//get the list for the block type
		List<MonsterBlock> lst = dictionary[block.BlockInfo.BlockID];
		//create one if needed
		if(lst == null) {
			lst = new List<MonsterBlock>();
			dictionary.Add(block.BlockInfo.BlockID, lst);
		}
		//add the block to the list
		lst.Add(block);
	}

	public void move(Vector3 pos, Vector3 offset){
		find(pos).move(offset);
	}
	
	public void move(MonsterBlock blk, Vector3 offset){
		blk.move(offset);
	}

	public void rotate(Vector3 pos, bool goClockwise = true){
		rotate(pos, goClockwise);
	}
	
	public void rotate(MonsterBlock blk, bool goClockwise = true){
		blk.rotate(goClockwise);
	}

	
	/*
	 * public bool remove (Vector3 position)
	 * 
	 * Remove a block from the map data.
	 */
	public void remove(Vector3 position) {
		remove(find(position));
	}
	
	public void remove(MonsterBlock blk){
		//unlink neighbors
		unlinkTerrain(blk);
		//remove from list
		dictionary[blk.BlockInfo.BlockID].Remove(blk);
	}


	#endregion Manipulation


	#region (un)linkTerrain
	/*
	 * private void linkTerrain (MonsterBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * Also links block as a neighbor to any adjacent blocks.
	 */
	private void linkTerrain(MonsterBlock block) {
		MapData.Instance.TerrainBlocks.find(block.Position).addMonster(block);
	}

	/*
	 * private void unlinkTerrain(MonsterBlock block)
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
	#endregion (un)linkTerrain


	/*
	 * public TerrainBlock findBlock (Vector3 position)
	 * 
	 * Returns the block at position
	 * Returns null if there is no block in that position.
	 */
	public MonsterBlock find(Vector3 position) {
		//for each type of block
		foreach(KeyValuePair<string, List<MonsterBlock>> kvPair in dictionary) {
			//check each block
			foreach(MonsterBlock blk in kvPair.Value) {
				//return block if position matches
				if(blk.Position.Equals(position)) {
					return blk;
				}//otherwise continue to next
			}
		}
		//return null if none found
		return null;
	}






	#region Validation
	public bool isAddValid(Vector3 pos){
		TerrainBlock blk = MapData.Instance.TerrainBlocks.find(pos);
		if(blk == null) {
			return false;
		}
		if(blk.Monster != null) {
			return false;
		}
		return blk.Pathable;
	}
	#endregion Validation

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