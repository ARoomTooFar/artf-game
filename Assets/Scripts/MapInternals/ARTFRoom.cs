using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ARTFRoom {

	private Vector3 LLposition = new Vector3();
	private Vector3 URposition = new Vector3();
	private List<TerrainBlock> blocks = new List<TerrainBlock>();
	private string defaultBlockID = "defaultBlockID";
	
	public Vector3 LLCorner {
		get { return LLposition; }
	}
	
	public Vector3 URCorner {
		get { return URposition; }
	}

	public Vector3 LRCorner {
		get { return new Vector3(URCorner.x, URCorner.y, LLCorner.z); }
	}

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

	public float Height {
		get { return 1 + URposition.z - LLposition.z; }
	}

	public float Length {
		get { return 1+ URposition.x - LLposition.x; }
	}
	
	/*
	 * Constructor
	 */
	public ARTFRoom(Vector3 pos1, Vector3 pos2) {
		this.LLposition = pos1.getMinVals(pos2);
		this.URposition = pos1.getMaxVals(pos2);
	}

	public void linkTerrain() {
		unlinkTerrain();
		TerrainBlock blk;
		Vector3 pos = new Vector3();
		for(int i = 0; i < Length; ++i) {
			for(int j = 0; j < Height; ++j) {
				pos.Set(i + LLCorner.x, 0, j + LLCorner.z);
				blk = MapData.Instance.TerrainBlocks.findBlock(pos);
				if(blk == null) {
					blk = new TerrainBlock(defaultBlockID, pos, DIRECTION.North);
					MapData.Instance.TerrainBlocks.addBlock(blk);
				}
				blocks.Add(blk);
				blk.Room = this;
			}
		}
	}

	public void unlinkTerrain() {
		foreach(TerrainBlock blk in blocks) {
			blk.Room = null;
		}
		blocks.Clear();
	}

	public bool Move(Vector3 offset) {
		LLposition = LLposition.Add(offset);
		URposition = URposition.Add(offset);
		//add offsets to all blocks
		foreach(TerrainBlock blk in blocks) {
			blk.move(offset);
		}
		//for each edge block
		//	for each neighbor
		foreach(TerrainBlock blk in blocks) {
			if(isEdge(blk.Position)) {
				MapData.Instance.TerrainBlocks.relinkNeighbors(blk);
			}
		}
		return true;
	}

	public bool Resize(Vector3 oldCorner, Vector3 newCorner) {
		if(!isCorner(oldCorner)) {
			return false;
		}
		Vector3 offset = newCorner.Subtract(oldCorner);
		if(oldCorner.x == LLposition.x) {
			LLposition.x += offset.x;
		} else {
			URposition.x += offset.x;
		}

		if(oldCorner.z == LLposition.z) {
			LLposition.z += offset.z;
		} else {
			URposition.z += offset.z;
		}
		//remove blocks no longer in room
		foreach(TerrainBlock blk in blocks) {
			if(!inRoom(blk.Position)) {
				MapData.Instance.TerrainBlocks.removeBlock(blk.Position);
			}
		}
		//relink blocks to this room
		linkTerrain();
		return true;
	}

	public bool Rotate(DIRECTION dir) {
		return false;
	}

	public bool isCorner(Vector3 pos) {
		if(LLposition.Equals(pos))
			return true;
		if(URposition.Equals(pos))
			return true;
		if(LLposition.x.Equals(pos.x) && URposition.z.Equals(pos.z))
			return true;
		if(URposition.x.Equals(pos.x) && LLposition.z.Equals(pos.z))
			return true;
		return false;
	}

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

	public bool inRoom(Vector3 pos) {
		return pos.z >= LLposition.z && pos.z <= URposition.z && pos.z >= LLposition.z && pos.z <= URposition.z;
	}

	public int numBlocks(){
		return blocks.Count;
	}
}
