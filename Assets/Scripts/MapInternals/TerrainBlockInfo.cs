using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TerrainBlockInfo {

	/*
	 * Constructor
	 */
	private TerrainBlockInfo(string blockID, bool pathable){
		this.blockid = blockID;
		this.pathable = pathable;
	}

	//static dictionary containing info for every block
	private static Dictionary<string, TerrainBlockInfo> infoDictionary;

	/*
	 * private static bool loadBlockInfo()
	 * 
	 * loads information about blocks from whatever file
	 */
	private static void loadBlockInfo(){
		infoDictionary = new Dictionary<string, TerrainBlockInfo> ();
		//load data
		infoDictionary.Add ("block1", new TerrainBlockInfo("block1", true));
		//return true;
	}
	
	/*
	 * public static TerrainBlockInfo get(string blockID)
	 * 
	 * Returns the info for the block with ID blockID.
	 * Returns null if info does not exist.
	 * 
	 * Also loads the data if it hasn't been loaded yet.
	 */
	public static TerrainBlockInfo get(string blockID){
		if (infoDictionary == null) {
			loadBlockInfo();
		}

		try{
			return infoDictionary[blockID];
		} catch ( Exception ){
			return null;
		}
	}

	//blockID of block
	private string blockid;
	public string BlockID{
		get{ return blockid;}
	}

	//true if block can be walked on
	private bool pathable;
	public bool Pathable{
		get{ return pathable; }
	}


}


