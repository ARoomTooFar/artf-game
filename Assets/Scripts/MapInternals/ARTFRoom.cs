using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ARTFRoom {
	
	private Vector3 LLposition = new Vector3 ();
	private Vector3 URposition = new Vector3 ();
	private List<TerrainBlock> blocks = new List<TerrainBlock>();
	
	public Vector3 LLPosition {
		get { return LLposition; }
	}
	
	public Vector3 URPosition {
		get { return URposition; }
	}
	
	public string SaveString{
		get{ return LLposition.toCSV () + "," + URposition.toCSV ();}
	}

	public float Area{
		get { return Length * Height; }
	}

	public float Perimeter{
		get { return 2 * (Length + Height); }
	}

	public float Height {
		get { return URposition.z - LLposition.z; }
	}

	public float Length {
		get { return URposition.x - LLposition.x; }
	}
	
	/*
	 * Constructor
	 */
	public ARTFRoom (Vector3 pos1, Vector3 pos2) {
		this.LLposition = pos1.getMinVals(pos2);
		this.URposition = pos1.getMaxVals(pos2);
	}

	public void linkTerrain(){
		blocks.Clear();
		for(int i = 0; i <= Length; ++i){
			for(int j = 0; j <= Height; ++j){
				blocks.Add(MapData.Instance.TerrainBlocks.findBlock(new Vector3(i+Length, 0, j+Height)));
			}
		}
	}

	public bool Move(Vector3 offset){
		LLposition = LLposition.Add(offset);
		URposition = URposition.Add(offset);
		//for each edge block
		//check for overlaps and clone
		//add offsets to all blocks
		//for each edge block
		//	for each neighbor
		//		if block is not part of room, break link
		//for each edge block
		//check for overlaps and merge
		return false;
	}

	public bool Resize(Vector3 oldCorner, Vector3 newCorner){
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
		//add blocks that are now in room
		return false;
	}

	public bool Rotate(DIRECTION dir){
		return false;
	}

	public bool isCorner(Vector3 pos){
		if (LLposition.Equals(pos)) return true;
		if (URposition.Equals(pos)) return true;
		if (LLposition.x.Equals(pos.x) && URposition.z.Equals(pos.z)) return true;
		if (URposition.x.Equals(pos.x) && LLposition.z.Equals(pos.z)) return true;
		return false;
	}

	public bool isEdge(Vector3 pos){
		if(pos.x.Equals(LLposition.x) || pos.x.Equals(URposition.x)) {
			if(pos.z >= LLposition.z && pos.z <= URposition.z){
				return true;
			}
		}
		if(pos.z.Equals(LLposition.z) || pos.z.Equals(URposition.z)) {
			if(pos.x >= LLposition.x && pos.x <= URposition.x){
				return true;
			}
		}
		return false;
	}

	public bool isInRoom(Vector3 pos){
		return pos.z >= LLposition.z && pos.z <= URposition.z && pos.z >= LLposition.z && pos.z <= URposition.z;
	}
}