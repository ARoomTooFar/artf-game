using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ARTFRoom {

	#region PrivateVariables
	private Vector3 LLposition = new Vector3();
	private Vector3 URposition = new Vector3();
	protected List<TerrainBlock> blocks = new List<TerrainBlock>();
	private string defaultBlockID = "defaultBlockID";
	#endregion PrivateVariables

	#region Properties
	//Lower Left Corner
	public Vector3 LLCorner {
		get { return LLposition; }
	}

	//Upper Right Corner
	public Vector3 URCorner {
		get { return URposition; }
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
	
	public string SaveString {
		get{ return LLposition.toCSV() + "," + URposition.toCSV();}
	}

	public float Area {
		get { return Length * Height; }
	}

	public float Perimeter {
		get { return 2 * (Length + Height); }
	}

	//Add 1 because a grid with corners in the same position has Length/Height == 1
	public float Height {
		get { return 1 + URposition.z - LLposition.z; }
	}

	public float Length {
		get { return 1+ URposition.x - LLposition.x; }
	}
	#endregion Properties
	
	/*
	 * Constructor
	 */
	public ARTFRoom(Vector3 pos1, Vector3 pos2) {
		this.LLposition = pos1.getMinVals(pos2);
		this.URposition = pos1.getMaxVals(pos2);
	}

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

	/*
	 * public bool Move(Vector3 offset)
	 * 
	 * Moves a room in the x/y/z direction specified
	 * by the offset Vector3
	 * 
	 * Returns true if successful
	 * Returns false otherwise 
	 */
	public bool Move(Vector3 offset) {
		//Shift the LowerLeft and UpperRight corners by offset
		LLposition = LLposition + offset;
		URposition = URposition + offset;
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
		return true;
	}

	/*
	 * public bool Resize(Vector3 oldCorner, Vector3 newCorner)
	 * 
	 * Resizes a room by moving one corner to a new position
	 * 
	 * Returns true if successful.
	 * Returns false otherwise.
	 */
	public bool Resize(Vector3 oldCorner, Vector3 newCorner) {
		//Make sure that the old corner is actually a corner
		if(!isCorner(oldCorner)) {
			return false;
		}
		//get the offset
		Vector3 offset = newCorner - oldCorner;
		//determine which corner to move in the x direction
		if(oldCorner.x == LLposition.x) {
			LLposition.x += offset.x;
		} else {
			URposition.x += offset.x;
		}
		//determine which corner to move in the z direction
		if(oldCorner.z == LLposition.z) {
			LLposition.z += offset.z;
		} else {
			URposition.z += offset.z;
		}
		//remove blocks no longer in room
		foreach(TerrainBlock blk in blocks) {
			if(!inRoom(blk.Position)) {
				MapData.Instance.TerrainBlocks.remove(blk.Position);
			}
		}
		//relink blocks to this room
		linkTerrain();
		return true;
	}

	public bool Rotate(DIRECTION dir) {
		return false;
	}

	/*
	 * public void Remove()
	 * 
	 * Removes all linked blocks from MapData
	 */
	public void Remove(){
		foreach(TerrainBlock blk in this.blocks){
			MapData.Instance.TerrainBlocks.remove(blk.Position);
		}
		this.blocks.Clear();
	}

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
		if(pos.x.Equals(LLposition.x) || pos.x.Equals(URposition.x)) {
			if(pos.z >= LLposition.z && pos.z <= URposition.z) {
				return true;
			}
		}
		if(pos.z.Equals(LLposition.z) || pos.z.Equals(URposition.z)) {
			if(pos.x >= LLposition.x && pos.x <= URposition.x) {
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
			pos.z >= LLposition.z &&
			pos.z <= URposition.z &&
			pos.z >= LLposition.z &&
			pos.z <= URposition.z;
	}
}
