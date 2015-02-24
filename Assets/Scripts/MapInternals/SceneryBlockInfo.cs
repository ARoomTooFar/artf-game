using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Class to hold information about scenery
 * 
 */
public class SceneryBlockInfo {

	/*
     * Constructor
     */
	private SceneryBlockInfo(string blockID, bool pathable, List<Vector3> coordinates) {
		this.BlockID = blockID;
		this.Pathable = pathable;
		this.localCoordinates = coordinates;
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
	public string BlockID {
		get;
		private set;
	}

	//true if block can be walked on/through

	public bool Pathable {
		get;
		private set;
	}

	private List<Vector3> localCoordinates;

	public List<Vector3> LocalCoordinates(DIRECTION dir) {
		List<Vector3> retVal = new List<Vector3>();
		foreach(Vector3 vec in localCoordinates) {
			retVal.Add(vec.RotateTo(dir));
		}
		return retVal;
	}


}


