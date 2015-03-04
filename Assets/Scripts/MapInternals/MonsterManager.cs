using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		//attempt to link the input to its neighbors
		linkTerrain(blk);
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
		blk.move(offset);
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
		//unlink neighbors
		//unlinkTerrain(blk);
		blk.remove();
		//remove from list
		dictionary[blk.BlockInfo.BlockID].Remove(blk);
	}
	#endregion Remove
	#endregion Manipulation


	#region (un)linkTerrain
	/*
	 * private void linkTerrain (MonsterBlock block)
	 * 
	 * Gets the adjacent blocks and adds them as neighbors to block.
	 * Also links block as a neighbor to any adjacent blocks.
	 */
	private void linkTerrain(MonsterBlock blk) {
		MapData.TerrainBlocks.find(blk.Position).addMonster(blk);
	}

	/*
	 * private void unlinkTerrain(MonsterBlock block)
	 * 
	 * Breaks all neighbor links between block and its list of neighbors.
	 * 
	 */
	private void unlinkTerrain(MonsterBlock blk) {
		TerrainBlock terBlk = MapData.TerrainBlocks.find(blk.Position);
		if(terBlk.Monster.Equals(blk)){
			terBlk.removeMonster();
		}
	}
	#endregion (un)linkTerrain


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
	public bool isAddValid(Vector3 pos){
		TerrainBlock blk = MapData.TerrainBlocks.find(pos);
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