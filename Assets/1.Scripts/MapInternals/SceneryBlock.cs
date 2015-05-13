using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a piece of scenery on the map
 * 
 */
public class SceneryBlock : GenericBlock {

	#region Properties

	public SceneryData SceneryBlockInfo {
		get { return GameObj.GetComponent<SceneryData>(); }
	}

	#endregion Properties

	/*
	 * Constructor
	 */
	public SceneryBlock (string blockID, Vector3 pos, DIRECTION dir) : base(blockID, pos, dir){}
	
	public Vector3 doorCheckPosition {
		get{return Position.moveinDir(Orientation);}
	}
}


