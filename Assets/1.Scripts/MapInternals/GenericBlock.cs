using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GenericBlock {

	#region Properties
	public LevelEntityData BlockInfo {
		get { return GameObj.GetComponent<LevelEntityData>(); }
	}

	public string BlockID{
		get{ return BlockInfo.BlockID;}
	}
	
	public Vector3 Position {
		get;
		protected set;
	}

	public DIRECTION Orientation {
		get;
		protected set;
	}

	public string SaveString{
		get{ return Position.toCSV () + "," + Orientation.ToString(); }
	}

	public GameObject GameObj {
		get;
		protected set;
	}

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
	public GenericBlock (string blockID, Vector3 pos, DIRECTION dir) {
		this.Position = pos.Round ();
		this.Orientation = dir;
		this.GameObj = GameObjectResourcePool.getResource(blockID, pos, dir.toRotationVector());
	}

	/*
	 * public void move(Vector3 offset)
	 * 
	 * Alters the position of the monster by offset
	 */
	public void move(Vector3 offset){
		Position += offset;
		GameObj.transform.position = Position;
	}

	/*
	 * public void rotate(bool goClockwise = true)
	 * 
	 * Rotates the monster
	 */
	public void rotate(bool goClockwise = true){
		Orientation = Orientation.QuarterTurn(goClockwise);
		GameObj.transform.eulerAngles = Orientation.toRotationVector();
	}

	public void rotate(DIRECTION orientation){
		Orientation = orientation;
		GameObj.transform.eulerAngles = Orientation.toRotationVector();
	}

	public void remove(){
		GameObjectResourcePool.returnResource(BlockInfo.BlockID, GameObj);
	}
}


