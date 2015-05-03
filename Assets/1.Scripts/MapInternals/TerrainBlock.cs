using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent one tile/block/terrainspace/thing in the map
 * 
 * Contains links to neighboring blocks as well as the scenery and/or monster on the tile
 * 
 */
public class TerrainBlock {

	#region Properties

	public string wallType = "{0}/Rooms/wallstoneend";

	public Dictionary<DIRECTION, TerrainBlock> Neighbors {
		get;
		private set;
	}

	public SceneryBlock Scenery {
		get;
		private set;
	}

	public SceneryBlock Wall {
		get;
		private set;
	}

	public ARTFRoom Room {
		get;
		set;
	}

	public Vector3 Position {
		get;
		set;
	}

	public DIRECTION Orientation {
		get;
		private set;
	}

	public string SaveString {
		get{ return Position.toCSV() + "," + Orientation.ToString() + "," + wallType;}
	}

	public String BlockID {
		get;
		private set;
	}

	#endregion Properties

	#region Constructors
	/*
	 * Constructor
	 */
	public TerrainBlock(string blockID, Vector3 pos, DIRECTION dir) {
		this.Position = pos.Round();
		this.Orientation = dir;
		this.Neighbors = new Dictionary<DIRECTION, TerrainBlock>();
		this.BlockID = blockID;
		this.Wall = null;
		//Debug.Log(GameObj.transform.position);
	}
	
	#endregion Constructors

	#region Neighbors
	/*
	 * public bool addNeighbor(TerrainBlock that, DIRECTION dir)
	 * 
	 * that becomes a neighboring block to this in direction dir.
	 * 
	 * returns true if successfully added.
	 * returns false if the block already has a neighbor in that direction.
	 * 
	 * Throws exception if dir is NotDirection.
	 * 
	 */
	public bool addNeighbor(TerrainBlock blk, DIRECTION dir) {
		if(dir.Equals(DIRECTION.NonDirectional)) {
			throw new Exception("Invalid DIRECTION");
		}
		try {
			Neighbors.Add(dir, blk);
		} catch(ArgumentException) {
			return false;
		}
		return true;
	}

	/*
	 * public bool removeNeighbor(DIRECTION dir)
	 * 
	 * removes the neighbor in direction dir
	 * 
	 * returns true if successfully removed.
	 * returns false if no neighbor was present.
	 * 
	 */
	public bool removeNeighbor(DIRECTION dir) {
		return Neighbors.Remove(dir);
	}

	/*
	 * public void clearNeighbors()
	 * 
	 * Removes all neighbors from TerrainBlock
	 * 
	 */
	public void clearNeighbors() {
		Neighbors.Clear();
	}


	/*
	 * public DIRECTION isNeighbor(TerrainBlock other)
	 * 
	 * Returns the DIRECTION in which other lies in relation to this.
	 * Returns DIRECTION.NotNeighbor if other is not adjacent.
	 */
	public DIRECTION isNeighbor(TerrainBlock other) {
		//get difference in position
		float xDif = other.Position.x - this.Position.x;
		float zDif = other.Position.z - this.Position.z;

		//cardinal directions
		if(xDif == 0 && zDif == 1) {
			return DIRECTION.North;
		}
		if(xDif == 0 && zDif == -1) {
			return DIRECTION.South;
		}
		if(xDif == 1 && zDif == 0) {
			return DIRECTION.East;
		}
		if(xDif == -1 && zDif == 0) {
			return DIRECTION.West;
		}

		//ordinal directions
		if(xDif == 1 && zDif == 1) {
			return DIRECTION.NorthEast;
		}
		if(xDif == 1 && zDif == -1) {
			return DIRECTION.SouthEast;
		}
		if(xDif == -1 && zDif == 1) {
			return DIRECTION.NorthWest;
		}
		if(xDif == -1 && zDif == -1) {
			return DIRECTION.SouthWest;
		}

		//default value of NotNeighbor
		return DIRECTION.NonDirectional;
	}
	#endregion Neighbors

	#region Scenery
	public bool addWall(DIRECTION dir) {

		//return false if there is already scenery
		if(this.Wall != null) {
			return false;
		}
		
		this.Wall = new SceneryBlock(wallType, this.Position, dir);
		return true;
	}

	public void removeWall(){
		if(this.Wall == null) {
			return;
		}
		this.Wall.remove();
		this.Wall = null;
	}
	#endregion Scenery

	#region Manipulation
	/*
	 * public void move(Vector3 offset)
	 * 
	 * moves the block and associated scenery and monster
	 */
	public void move(Vector3 offset) {
		if(this.Wall != null) {
			this.Wall.move(offset);
		}

		Position += offset;
		//GameObj.transform.position = Position;

	}
	
	public void remove() {
		if(Wall != null) {
			this.removeWall();
		}
	}
	#endregion Manipulation
}


