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
	public MonsterBlockInfo BlockInfo {
		get;
		private set;
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
	#endregion Properties

	/*
	 * Constructor
	 */
	public MonsterBlock (string blockID, Vector3 pos, DIRECTION dir) {
		this.BlockInfo = MonsterBlockInfo.get (blockID);
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


