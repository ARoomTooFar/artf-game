using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a monster on the map
 * 
 */
public class MonsterBlock {

	#region PrivateVariables
	private MonsterBlockInfo blockInfo;
	private Vector3 position = new Vector3 ();
	private DIRECTION orientation;
	#endregion PrivateVariables

	#region Properties
	public MonsterBlockInfo BlockInfo {
		get{ return blockInfo; }
	}
	
	public Vector3 Position {
		get { return position; }
	}

	public DIRECTION Orientation{
		get{ return orientation; }
	}

	public string SaveString{
		get{ return position.toCSV () + "," + Orientation.ToString(); }
	}
	#endregion Properties

	/*
	 * Constructor
	 */
	public MonsterBlock (string blockID, Vector3 pos, DIRECTION orientation) {
		this.blockInfo = MonsterBlockInfo.get (blockID);
		this.position = pos.Round ();
		this.orientation = orientation;
	}

	/*
	 * public void move(Vector3 offset)
	 * 
	 * Alters the position of the monster by offset
	 */
	public void move(Vector3 offset){
		position = position + offset ;
	}

	public void rotate(bool goClockwise = true){
		orientation = orientation.QuarterTurn(goClockwise);
	}
}


