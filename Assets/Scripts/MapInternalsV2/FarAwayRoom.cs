using System;
using UnityEngine;

public class FarAwayRoom {
	public FarAwayRoom(Vector3 llcorner, Vector3 urcorner) {
		LLCorner = llcorner;
		URCorner = urcorner;
	}
	
	public Vector3 LLCorner { get; private set; }
	public Vector3 URCorner { get; private set; }
	
	//Lower Right Corner
	public Vector3 LRCorner {
		get { return new Vector3(URCorner.x, URCorner.y, LLCorner.z); }
	}
	
	//Upper Right Corner
	public Vector3 ULCorner {
		get { return new Vector3(LLCorner.x, URCorner.y, URCorner.z); }
	}
	
	public Vector3 Center {
		get { return (LLCorner + URCorner) / 2; }
	}
	
	public GameObject LLMarker;
	public GameObject URMarker;
	public GameObject ULMarker;
	public GameObject LRMarker;
	
	#region SquareProperties
	public float Area {
		get { return Length * Height; }
	}
	
	public float Perimeter {
		get { return 2 * (Length + Height); }
	}
	
	//Add 1 because a grid with corners in the same position has Length/Height == 1
	public float Height {
		get { return 1 + URCorner.z - LLCorner.z; }
	}
	
	public float Length {
		get { return 1 + URCorner.x - LLCorner.x; }
	}
	#endregion SquareProperties
	
	#region PositionChecks
	/*
	 * public bool isCorner(Vector3 pos)
	 * 
	 * Check if a position is a corner position
	 */
	public bool isCorner(Vector3 pos) {
		if(LLCorner.Equals(pos))
			return true;
		if(URCorner.Equals(pos))
			return true;
		if(LRCorner.Equals(pos))
			return true;
		if(ULCorner.Equals(pos))
			return true;
		return false;
	}
	
	/*
	 * public bool isEdge(Vector3 pos)
	 * 
	 * Check if a position is on one of the outermost tiles
	 */
	public bool isEdge(Vector3 pos) {
		if(pos.x.Equals(LLCorner.x) || pos.x.Equals(URCorner.x)) {
			if(pos.z >= LLCorner.z && pos.z <= URCorner.z) {
				return true;
			}
		}
		if(pos.z.Equals(LLCorner.z) || pos.z.Equals(URCorner.z)) {
			if(pos.x >= LLCorner.x && pos.x <= URCorner.x) {
				return true;
			}
		}
		return false;
	}
	
	/*
	 * public DIRECTION getWallSide(Vector3 pos)
	 * 
	 * Returns a direction corresponding to the which wall the position is in 
	 * 
	 */
	public DIRECTION getWallSide(Vector3 pos){
		//If not an edge, return non-directional
		if(!isEdge(pos)) {
			return DIRECTION.NonDirectional;
		}
		DIRECTION NSDir = DIRECTION.NonDirectional;
		DIRECTION EWDir = DIRECTION.NonDirectional;
		
		if(pos.x == LLCorner.x) {
			EWDir = DIRECTION.West;
		}
		if(pos.x == URCorner.x) {
			EWDir = DIRECTION.East;
		}
		if(pos.z == LLCorner.z) {
			NSDir = DIRECTION.South;
		}
		if(pos.z == URCorner.z) {
			NSDir = DIRECTION.North;
		}
		//Debug.Log(LLCorner + ", " + URCorner + ", " + pos + ", " + NSDir.ToString() + ", " + EWDir.ToString());
		if(EWDir.Equals(DIRECTION.NonDirectional)) {
			return NSDir;
		}
		if(NSDir.Equals(DIRECTION.NonDirectional)) {
			return EWDir;
		}
		
		return NSDir.getOrdinalFromCardinals(EWDir);
	}
	
	/*
	 * public bool inRoom(Vector3 pos)
	 * 
	 * Check if a given position is on a tile in this room
	 */
	public bool inRoom(Vector3 pos) {
		return
			pos.x >= LLCorner.x &&
				pos.x <= URCorner.x &&
				pos.z >= LLCorner.z &&
				pos.z <= URCorner.z;
	}
	#endregion PositionChecks
}
