using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Enum for cardinal and ordinal directions
 * 
 * Directions opposite each other on a compass
 * are set to be negative values of each other to
 * make finding the opposite direction easier.
 * 
 */
public enum DIRECTION : int {
	NonDirectional = 0,
	North = 1,
	South = -(int)North,
	East = 2,
	West = -(int)East,
	NorthEast = 3,
	SouthWest = -(int)NorthEast,
	SouthEast = 4,
	NorthWest = -(int)SouthEast,
};

/*
 * Object to represent one tile/block/terrainspace/thing in the map
 * 
 * Contains links to neighboring blocks as well as the scenery and/or monster on the tile
 * 
 */
public class TerrainBlock {
	#region PrivateVariables
	private Dictionary<DIRECTION, TerrainBlock> neighbors = new Dictionary<DIRECTION, TerrainBlock>();
	private SceneryBlock scenery;
	private MonsterBlock monster;
	private ARTFRoom room;
	private Vector3 position = new Vector3();
	private DIRECTION orientation;
	private TerrainBlockInfo blockInfo;
	#endregion PrivateVariables

	#region Properties
	public Dictionary<DIRECTION, TerrainBlock> Neighbors {
		get { return neighbors; }
	}

	public SceneryBlock Scenery {
		get{ return scenery; }
	}

	public MonsterBlock Monster {
		get{ return monster; }
	}

	public ARTFRoom Room {
		get{ return room; } 
		set{ room = value; }
	}

	public Vector3 Position {
		get { return position; }
		set { position = value.Round(); }
	}

	public DIRECTION Orientation {
		get { return orientation; }
	}

	public TerrainBlockInfo BlockInfo {
		get{ return blockInfo; }
	}

	public string SaveString {
		get{ return position.toCSV() + "," + orientation.ToString();}
	}
	#endregion Properties

	/*
	 * Constructor
	 */
	public TerrainBlock(string blockID, Vector3 position, DIRECTION orientation) {
		this.blockInfo = TerrainBlockInfo.get(blockID);
		this.position = position.Round();
		this.orientation = orientation;
	}

	/*
	 * Deep Copy constructor
	 */
	public TerrainBlock(TerrainBlock original){
		this.neighbors = new Dictionary<DIRECTION, TerrainBlock>(original.neighbors);
		this.scenery = original.scenery;
		this.monster = original.monster;
		this.room = original.room;
		this.position = original.position.Copy();
		this.orientation = original.orientation;
		this.blockInfo = original.blockInfo;
	}

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
	public bool addNeighbor(TerrainBlock that, DIRECTION dir) {
		if(dir.Equals(DIRECTION.NonDirectional)) {
			throw new Exception("Invalid DIRECTION");
		}
		try {
			neighbors.Add(dir, that);
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
		return neighbors.Remove(dir);
	}

	/*
	 * public void clearNeighbors()
	 * 
	 * Removes all neighbors from TerrainBlock
	 * 
	 */
	public void clearNeighbors() {
		neighbors.Clear();
	}

	/*
	 * public TerrainBlock getNeighbor(DIRECTION dir)
	 * 
	 * Returns the neighboring TerrainBlock in direction dir.
	 * Returns null if there is no block in that direction
	 */
	public TerrainBlock getNeighbor(DIRECTION dir) {
		try {
			return neighbors[dir];
		} catch(Exception) {
			return null;
		}
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

	/*
	 * public bool addScenery(SceneryBlock scenery)
	 * 
	 * Links this block to a piece of scenery
	 * 
	 * Returns true if successfully linked
	 * Returns false if not.
	 */
	public bool addScenery(SceneryBlock scenery) {
		//return false if there is already scenery
		if(this.scenery != null) {
			return false;
		}

		//if the scenery blocks movement and there is a monster, return false
		if(!scenery.BlockInfo.Pathable && this.monster != null) {
			return false;
		}
		
		this.scenery = scenery;
		return true;
	}

	/*
	 * public void removeScenery()
	 * 
	 * Unlinks the piece of scenery linked to this block
	 */
	public void removeScenery() {
		this.scenery = null;
	}

	/*
	 * public bool addMonster(MonsterBlock monster)
	 * 
	 * Links this block to a monster
	 * 
	 * Returns true if successfully linked
	 * Returns false if not.
	 */
	public bool addMonster(MonsterBlock monster) {
		//return false if there is already a monster linked
		if(this.monster != null) {
			return false;
		}

		//return false if there is a piece of scenery that blocks pathing
		if(this.scenery != null && !this.scenery.BlockInfo.Pathable) {
			return false;
		}
		
		this.monster = monster;
		return true;
	}

	/*
	 * public void removeMonster()
	 * 
	 * Unlinks the monster linked to this block
	 */
	public void removeMonster() {
		this.monster = null;
	}

	/*
	 * public void move(Vector3 offset)
	 * 
	 * moves the block and associated scenery and monster
	 */
	public void move(Vector3 offset){
		if(this.scenery != null && this.scenery.Position.Equals(this.position)) {
			this.scenery.move(offset);
		}

		if(this.monster != null) {
			this.monster.move(offset);
		}

		position = position + offset;

	}
}


