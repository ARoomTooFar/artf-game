using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square {

	public Square(Vector3 llcorner, Vector3 urcorner){
		LLCorner = llcorner.getMinVals (urcorner);
		URCorner = llcorner.getMaxVals (urcorner);
	}

	#region Corners
	//Lower Left Corner
	public Vector3 LLCorner {
		get;
		private set;
	}
	
	//Upper Right Corner
	public Vector3 URCorner {
		get;
		private set;
	}
	
	//Lower Right Corner
	public Vector3 LRCorner {
		get { return new Vector3(URCorner.x, URCorner.y, LLCorner.z); }
	}
	
	//Upper Right Corner
	public Vector3 ULCorner {
		get { return new Vector3(LLCorner.x, URCorner.y, URCorner.z); }
	}
	
	//A list of all four corners
	public List<Vector3> Corners {
		get {
			List<Vector3> retVal = new List<Vector3>();
			retVal.Add(LLCorner);
			retVal.Add(URCorner);
			retVal.Add(LRCorner);
			retVal.Add(ULCorner);
			return retVal;
		}
	}
	
	//Center of the room. May be on the edge of two blocks in even sized rooms
	public Vector3 Center {
		get { return (LLCorner + URCorner) / 2; }
	}
	#endregion Corners

	#region SquareProperties
	public float Area {
		get { return Length * Height; }
	}

	public float UsableArea{
		get { return Area - Perimeter;}
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

	public int Cost {
		get { return Mathf.RoundToInt((10 * Mathf.Pow(2, (Mathf.Sqrt(UsableArea)) - 7) + 25) * 10); }
	}
	
	public int Points{
		get { return Mathf.RoundToInt(Mathf.Min(Length, Height)/20*UsableArea);}
	}
	#endregion SquareProperties

	public bool Intersect(Square that) {
		//for each corner in room1
		foreach(Vector3 cor in this.Corners) {
			//if that corner is inside room2
			if(that.inSquare(cor)) {
				return true;
			}
		}
		//for each corner in room2
		foreach(Vector3 cor in that.Corners) {
			//if that corner is inside room1
			if(this.inSquare(cor)) {
				return true;
			}
		}
		return false;
	}

	public bool inSquare(Vector3 pos) {
		return
			pos.x >= LLCorner.x &&
				pos.x <= URCorner.x &&
				pos.z >= LLCorner.z &&
				pos.z <= URCorner.z;
	}

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

	public virtual void resize(Vector3 oldCor, Vector3 newCor) {
		//Make sure that the old corner is actually a corner
		if(!isCorner(oldCor)) {
			return;
		}
		//get the offset
		Vector3 offset = newCor - oldCor;
		//determine which corner to move in the x direction
		if(oldCor.x == LLCorner.x) {
			LLCorner += new Vector3(offset.x, 0, 0);
		} else {
			URCorner += new Vector3(offset.x, 0, 0);
		}
		//determine which corner to move in the z direction
		if(oldCor.z == LLCorner.z) {
			LLCorner += new Vector3(0, 0, offset.z);
		} else {
			URCorner += new Vector3(0, 0, offset.z);
		}
	}

	public virtual void move(Vector3 offset){
		//Debug.Log(offset);
		LLCorner = LLCorner + offset;
		URCorner = URCorner + offset;
	}
}
