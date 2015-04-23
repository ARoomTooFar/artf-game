using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a piece of scenery on the map
 * 
 */
public class SceneryBlock {

	#region Properties

	public SceneryData BlockInfo {
		get { return GameObj.GetComponent<SceneryData>(); }
	}

	public string BlockID {
		get { return BlockInfo.BlockID; }
	}

	public Vector3 Position {
		get;
		private set;
	}

	public DIRECTION Orientation {
		get;
		private set;
	}

	public string SaveString{
		get{ return Position.toCSV () + "," + Orientation.ToString(); }
	}

	public GameObject GameObj {
		get;
		private set;
	}

	public bool Pathable {
		get { return BlockInfo.Pathable; }
	}

	public bool Walkable {
		get { return BlockInfo.Walkable; }
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
			foreach(Vector3 vec in BlockInfo.LocalCoordinates(Orientation)){
				//shift it to the global coordinate
				retVal.Add(vec + Position);
			}
			//return the list
			return retVal;
		}
	}
	#endregion Properties

	/*
	 * Constructor
	 */
	public SceneryBlock (string blockID, Vector3 pos, DIRECTION dir) {
		this.Position = pos.Round ();
		this.Orientation = dir;
		this.GameObj = GameObjectResourcePool.getResource(blockID, pos, dir.toRotationVector());
	}

	/*
	 * public void move(Vector3 offset)
	 * 
	 * Alters the position of the scenery by offset
	 */
	public void move(Vector3 offset){
		Position += offset;
		GameObj.transform.position = Position;
	}

	public void rotate(bool goClockwise = true){
		Orientation = Orientation.QuarterTurn(goClockwise);
		GameObj.transform.eulerAngles = Orientation.toRotationVector();
	}

	public void rotate(DIRECTION orientation){
		Orientation = orientation;
		GameObj.transform.eulerAngles = Orientation.toRotationVector();
	}

	public void remove(){
		GameObjectResourcePool.returnResource(BlockID, GameObj);
	}

	public Vector3 doorCheckPosition {
		get{return Position.moveinDir(Orientation);}
	}
}


