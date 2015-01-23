using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterBlock {
	public MonsterBlock () {
	}

	private MonsterBlockInfo blockInfo;
	public MonsterBlockInfo BlockInfo {
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
	public MonsterBlock (string blockID, Vector3 position, DIRECTION orientation) {
		this.blockInfo = MonsterBlockInfo.get (blockID);
		this.position = position.Round ();
		this.orientation = orientation;
	}
}


