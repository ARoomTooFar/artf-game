using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SceneryBlock {
	public SceneryBlock () {
	}

	private SceneryBlockInfo blockInfo;
	public SceneryBlockInfo BlockInfo {
		get{ return blockInfo; }
	}

	private Vector3 position = new Vector3 ();
	
	public Vector3 Position {
		get {
			return position;
		}
	}

	private DIRECTION orientation;
	public DIRECTION Orientation{
		get{return Orientation;}
	}

	public string SaveString{
		get{ return position.toCSV () + "," + Orientation.ToString();}
	}

	/*
	 * Constructor
	 */
	public SceneryBlock (string blockID, Vector3 position, DIRECTION orientation) {
		this.blockInfo = SceneryBlockInfo.get (blockID);
		this.position = position.Round ();
		this.orientation = orientation;
	}

	public List<Vector3> Coordinates{
		get{
			List<Vector3> retVal = blockInfo.LocalCoordinates(Orientation);
			foreach(Vector3 vec in retVal){
				vec.Add(position);
			}
			return retVal;
		}
	}
}


