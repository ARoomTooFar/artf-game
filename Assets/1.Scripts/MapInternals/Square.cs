using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square {

	public Square(Vector3 llcorner, Vector3 urcorner) {
		LLCorner = llcorner.getMinVals(urcorner);
		URCorner = llcorner.getMaxVals(urcorner);
		Corners = new List<Vector3>();
		setCorners();
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
	
	//Upper Left Corner
	public Vector3 ULCorner {
		get { return new Vector3(LLCorner.x, URCorner.y, URCorner.z); }
	}
	
	//A list of all four corners
	public List<Vector3> Corners { get; protected set; }
	
	//Center of the room. May be on the edge of two blocks in even sized rooms
	public Vector3 Center {
		get { return (LLCorner + URCorner) / 2; }
	}
	#endregion Corners

	#region SquareProperties
	public float Area {
		get { return Length * Height; }
	}

	public float UsableArea {
		get { return Area - Perimeter;}
	}
	
	public float Perimeter {
		get { return (2 * (Length + Height)) - 4; }
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
	
	public int Points {
		get { return Mathf.RoundToInt(Mathf.Min(Length, Height) / 20 * UsableArea);}
	}
	#endregion SquareProperties

	public bool Intersect(Square that) {
		return (this.LLCorner.x <= that.URCorner.x && this.URCorner.x >= that.LLCorner.x &&
			this.LLCorner.z <= that.URCorner.z && this.URCorner.z >= that.LLCorner.z);
	}

	public bool inSquare(Vector3 pos) {
		return
			pos.x >= LLCorner.x &&
			pos.x <= URCorner.x &&
			pos.z >= LLCorner.z &&
			pos.z <= URCorner.z;
	}

	public bool isCorner(Vector3 pos) {
		return Corners.Contains(pos);
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
		setCorners();
	}

	public virtual void move(Vector3 offset) {
		//Debug.Log(offset);
		LLCorner = LLCorner + offset;
		URCorner = URCorner + offset;
		setCorners();
	}

	public void setCorners() {
		Corners.Clear();
		Corners.Add(LLCorner);
		Corners.Add(URCorner);
		Corners.Add(LRCorner);
		Corners.Add(ULCorner);
	}
}
