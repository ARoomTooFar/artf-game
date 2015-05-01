using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a monster on the map
 * 
 */
public class MonsterBlock {

	#region Properties
	public MonsterData BlockInfo {
		get { return GameObj.GetComponent<MonsterData>(); }
	}

	public int Tier{
		get{ return BlockInfo.tier; }
		set{ BlockInfo.tier = value; }
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
		get{ return Position.toCSV () + "," + Orientation.ToString() + ", " + Tier; }
	}

	public GameObject GameObj {
		get;
		private set;
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

	public List<Vector3> RadiusCoordinates{
		get{
			//get the local coordinates this piece of scenery occupies in a given rotation
			List<Vector3> retVal = new List<Vector3>();
			//for each coordinate
			foreach(Vector3 vec in BlockInfo.RadiusCoordinates(Orientation)){
				//shift it to the global coordinate
				retVal.Add(vec + Position);
			}
			//return the list
			retVal.AddRange(Coordinates);
			return retVal;
		}
	}
	#endregion Properties

	/*
	 * Constructor
	 */
	public MonsterBlock (string blockID, Vector3 pos, DIRECTION dir) {
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

	public void remove(){
		GameObjectResourcePool.returnResource(BlockInfo.BlockID, GameObj);
	}
}


