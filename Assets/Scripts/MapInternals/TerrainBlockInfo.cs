using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TerrainBlockInfo {

	/*
	 * Constructor
	 */
	private TerrainBlockInfo(string blockID, bool pathable){
		this.BlockID = blockID;
		this.Pathable = pathable;
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
		infoDictionary.Add ("defaultBlockID", new TerrainBlockInfo("defaultBlockID", true));
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
	public string BlockID {
		get;
		private set;
	}

	//true if block can be walked on
	public bool Pathable {
		get;
		private set;
	}


}


