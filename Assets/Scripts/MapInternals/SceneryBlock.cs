using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a piece of scenery on the map
 * 
 */
public class SceneryBlock {

	#region PrivateVariables
	private SceneryBlockInfo blockInfo;
	private Vector3 position = new Vector3 ();
	private DIRECTION orientation;
	#endregion PrivateVariables

	#region Properties
	public SceneryBlockInfo BlockInfo {
		get{ return blockInfo; }
	}

	public Vector3 Position {
		get { return position; }
	}

	public DIRECTION Orientation{
		get{ return orientation; }
		set{ orientation = value; }
	}

	public string SaveString{
		get{ return position.toCSV () + "," + Orientation.ToString(); }
	}

	/*
	 * public List<Vector3> Coordinates
	 * 
	 * A list of global coordinates this piece of scenery occupies.
	 * Read-Only
	 */
	public List<Vector3> Coordinates{
		get{
			//get the local coordinates this piece of scenery occupies in a given rotation
			List<Vector3> retVal = new List<Vector3>();
			//for each coordinate
			foreach(Vector3 vec in blockInfo.LocalCoordinates(Orientation)){
				//shift it to the global coordinate
				retVal.Add(vec + position);
			}
			//return the list
			return retVal;
		}
	}
	#endregion Properties

	/*
	 * Constructor
	 */
	public SceneryBlock (string blockID, Vector3 position, DIRECTION orientation) {
		this.blockInfo = SceneryBlockInfo.get (blockID);
		this.position = position.Round ();
		this.orientation = orientation;
	}

	/*
	 * public void move(Vector3 offset)
	 * 
	 * Alters the position of the scenery by offset
	 */
	public void move(Vector3 offset){
		position = position + offset;
	}

	public void rotate(bool goClockwise = true){
		orientation = orientation.QuarterTurn(goClockwise);
	}
}


