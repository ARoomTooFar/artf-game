using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ARTFRoom {

	#region PrivateVariables
	protected List<TerrainBlock> blocks = new List<TerrainBlock>();
	private string defaultBlockID = "defaultBlockID";
	#endregion PrivateVariables

	#region Properties
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
	#endregion Corners

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
		get { return 1+ URCorner.x - LLCorner.x; }
	}
	#endregion SquareProperties

	public string SaveString {
		get{ return LLCorner.toCSV() + "," + URCorner.toCSV();}
	}
	#endregion Properties
	
	/*
	 * Constructor
	 */
	public ARTFRoom(Vector3 pos1, Vector3 pos2) {
		this.LLCorner = pos1.getMinVals(pos2);
		this.URCorner = pos1.getMaxVals(pos2);
	}

	#region (un)linkTerrain
	/*
	 * public void linkTerrain() 
	 * 
	 * Grabs any existing terrain that falls within the
	 * room's boundries and links them.
	 * 
	 * Creates new terrain using a default block if it
	 * does not already exist.
	 */
	public void linkTerrain() {
		//Undo all extant links to terrain
		unlinkTerrain();
		TerrainBlock blk;
		Vector3 pos = new Vector3();
		//for each coordinate in this room
		for(int i = 0; i < Length; ++i) {
			for(int j = 0; j < Height; ++j) {
				//set a Vector3 to the correct position
				pos.Set(i + LLCorner.x, 0, j + LLCorner.z);
				//try to find an existing block at that coordinate
				blk = MapData.Instance.TerrainBlocks.find(pos);
				//if it doesn't exist
				if(blk == null) {
					//create a new one
					blk = new TerrainBlock(defaultBlockID, pos, DIRECTION.North);
					//and add it to the MapData
					MapData.Instance.TerrainBlocks.add(blk);
				}
				//link the block to the room
				blocks.Add(blk);
				//and link the room to the block
				blk.Room = this;
			}
		}
	}

	/*
	 * public void unlinkTerrain()
	 * 
	 * Breaks any links between the room and its
	 * currently linked blocks
	 */
	public void unlinkTerrain() {
		//for each linked block
		foreach(TerrainBlock blk in blocks) {
			//unlink the room from the block
			blk.Room = null;
		}
		//remove all the links to terrain
		blocks.Clear();
	}
	#endregion (un)linkTerrain

	#region ManipulationFunctions
	/*
	 * public void move(Vector3 offset)
	 * 
	 * Moves a room in the x/y/z direction specified
	 * by the offset Vector3
	 */
	public void move(Vector3 offset) {
		//Shift the LowerLeft and UpperRight corners by offset
		LLCorner = LLCorner + offset;
		URCorner = URCorner + offset;
		//move each block by offset
		foreach(TerrainBlock blk in blocks) {
			blk.move(offset);
		}
		//for each block
		foreach(TerrainBlock blk in blocks) {
			//if it is an edge block
			if(isEdge(blk.Position)) {
				//force it to re-identify its neighbors
				MapData.Instance.TerrainBlocks.relinkNeighbors(blk);
			}
		}
	}

	/*
	 * public void resize(Vector3 oldCorner, Vector3 newCorner)
	 * 
	 * Resizes a room by moving one corner to a new position
	 */
	public void resize(Vector3 oldCor, Vector3 newCor) {
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
		//remove blocks no longer in room
		foreach(TerrainBlock blk in blocks) {
			if(!inRoom(blk.Position)) {
				MapData.Instance.TerrainBlocks.remove(blk.Position);
			}
		}
		//relink blocks to this room
		linkTerrain();
	}

	/*
	 * public void remove()
	 * 
	 * Removes all linked blocks from MapData
	 */
	public void remove(){
		foreach(TerrainBlock blk in this.blocks){
			MapData.Instance.TerrainBlocks.remove(blk.Position);
		}
		this.blocks.Clear();
	}
	#endregion ManipulationFunctions

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
