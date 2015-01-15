using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneryBlockInfo {

	/*
     * Constructor
     */
	private SceneryBlockInfo(string blockID, bool pathable, List<Vector3> coordinates) {
		this.blockid = blockID;
		this.pathable = pathable;
		this.coordinates = coordinates;
	}

	//static dictionary containing info for every block
	private static Dictionary<string, SceneryBlockInfo> infoDictionary;

	/*
     * private static bool loadBlockInfo()
     * 
     * loads information about blocks from whatever file
     */
	private static void loadBlockInfo() {
		infoDictionary = new Dictionary<string, SceneryBlockInfo>();
		//load data
		infoDictionary.Add("scenery1", new SceneryBlockInfo("block1", true, new List<Vector3>()));
		//return true;
	}
    
	/*
     * public static SceneryBlockInfo get(string blockID)
     * 
     * Returns the info for the block with ID blockID.
     * Returns null if info does not exist.
     * 
     * Also loads the data if it hasn't been loaded yet.
     */
	public static SceneryBlockInfo get(string blockID) {
		if(infoDictionary == null) {
			loadBlockInfo();
		}

		try {
			return infoDictionary[blockID];
		} catch(Exception) {
			return null;
		}
	}

	//blockID of block
	private string blockid;

	public string BlockID {
		get{ return blockid;}
	}

	//true if block can be walked on/through
	private bool pathable;

	public bool Pathable {
		get{ return pathable; }
	}

	private List<Vector3> coordinates;

	public List<Vector3> Coordinates(DIRECTION dir) {
		List<Vector3> retVal = new List<Vector3>();
		foreach(Vector3 vec in coordinates) {
			retVal.Add(vec.RotateTo(dir));
		}
		return retVal;
	}


}


