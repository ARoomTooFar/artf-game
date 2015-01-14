using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Enum for cardinal and ordinal directions
 * 
 * Directions opposite each other on a compass
 * are set to be negative values of each other to
 * make finding the opposite direction easier
 */
public enum DIRECTION : int {
	NotNeighbor = 0,
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
 * 
 */
public class TerrainBlock {
	private Dictionary<DIRECTION, TerrainBlock> neighbors = new Dictionary<DIRECTION, TerrainBlock> ();

	public Dictionary<DIRECTION, TerrainBlock> Neighbors {
		get {
			return neighbors;
		}
	}

	private Vector3 position = new Vector3 ();

	public Vector3 Position {
		get {
			return position;
		}
	}

	private Vector3 rotation = new Vector3 ();

	public Vector3 Rotation {
		set {
			rotation = value.Round ();
		}
		get {
			return rotation;
		}
	}

	private TerrainBlockInfo blockInfo;

	public TerrainBlockInfo BlockInfo {
		get{ return blockInfo; }
	}

	public string SaveString{
		get{ return position.toCSV () + "," + rotation.toCSV ();}
	}

	/*
	 * Constructor
	 */
	public TerrainBlock (string blockID, Vector3 position, Vector3 rotation) {
		this.blockInfo = TerrainBlockInfo.get (blockID);
		this.position = position.Round ();
		this.Rotation = rotation;
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
	public bool addNeighbor (TerrainBlock that, DIRECTION dir) {
		if(dir.Equals(DIRECTION.NotNeighbor)){
			throw new Exception("Invalid DIRECTION");
		}
		try {
			neighbors.Add (dir, that);
		} catch (ArgumentException) {
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
	public bool removeNeighbor (DIRECTION dir) {
		return neighbors.Remove (dir);
	}


	/*
	 * public void clearNeighbors()
	 * 
	 * Removes all neighbors from TerrainBlock
	 * 
	 */
	public void clearNeighbors () {
		foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION))) {
			neighbors.Remove (dir);
		}
	}

	/*
	 * public TerrainBlock getNeighbor(DIRECTION dir)
	 * 
	 * Returns the neighboring TerrainBlock in direction dir.
	 * Returns null if there is no block in that direction
	 */
	public TerrainBlock getNeighbor (DIRECTION dir) {
		try {
			return neighbors [dir];
		} catch (Exception) {
			return null;
		}
	}

	/*
	 * public DIRECTION isNeighbor(TerrainBlock other)
	 * 
	 * Returns the DIRECTION in which other lies in relation to this.
	 * Returns DIRECTION.NotNeighbor if other is not adjacent.
	 */
	public DIRECTION isNeighbor (TerrainBlock other) {
		float xDif = other.Position.x - this.Position.x;
		float zDif = other.Position.z - this.Position.z;

		if (xDif == 0 && zDif == 1) {
			return DIRECTION.North;
		}
		if (xDif == 0 && zDif == -1) {
			return DIRECTION.South;
		}
		if (xDif == 1 && zDif == 0) {
			return DIRECTION.East;
		}
		if (xDif == -1 && zDif == 0) {
			return DIRECTION.West;
		}

		if (xDif == 1 && zDif == 1) {
			return DIRECTION.NorthEast;
		}
		if (xDif == 1 && zDif == -1) {
			return DIRECTION.SouthEast;
		}
		if (xDif == -1 && zDif == 1) {
			return DIRECTION.NorthWest;
		}
		if (xDif == -1 && zDif == -1) {
			return DIRECTION.SouthWest;
		}
		return DIRECTION.NotNeighbor;
	}


}


