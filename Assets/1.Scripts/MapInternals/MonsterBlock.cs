using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a monster on the map
 * 
 */
public class MonsterBlock : GenericBlock {

	#region Properties
	public MonsterData MonsterBlockInfo {
		get { return GameObj.GetComponent<MonsterData>(); }
	}

	public new string SaveString{
		get{ return base.SaveString + ", " + MonsterBlockInfo.Tier; }
	}

	public List<Vector3> RadiusCoordinates{
		get{
			//get the local coordinates this piece of scenery occupies in a given rotation
			List<Vector3> retVal = new List<Vector3>();
			//for each coordinate
			foreach(Vector3 vec in MonsterBlockInfo.LocalRadiusCoordinates(Orientation)){
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
	public MonsterBlock (string blockID, Vector3 pos, DIRECTION dir) : base(blockID, pos, dir) {}
}


