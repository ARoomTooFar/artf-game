using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ARTFRoom {

	#region PrivateVariables
	private static string defaultBlockID = "Prefabs/Rooms/floortile";
	private static string defaultWall = "Prefabs/Rooms/wallstoneend";
	#endregion PrivateVariables

	#region Properties
	//A list of the TerrainBlocks contained within the room
	public List<TerrainBlock> Blocks {
		get;
		private set;
	}

	//A list of the other rooms this room is linked to
	public Dictionary<SceneryBlock, ARTFRoom> LinkedRooms {
		get;
		private set;
	}

	public List<SceneryBlock> Doors {
		get;
		private set;
	}

	//Stored paths to get from room A to room B
	public Dictionary<KeyValuePair<ARTFRoom, ARTFRoom>, List<Vector3>> RoomPaths {
		get;
		private set;
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

	//Center of the room. May be on the edge of two blocks in even sized rooms
	public Vector3 Center {
		get { return (LLCorner + URCorner) / 2; }
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
		get { return 1 + URCorner.x - LLCorner.x; }
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
		this.Blocks = new List<TerrainBlock>();
		this.LinkedRooms = new Dictionary<SceneryBlock, ARTFRoom>();
		this.Doors = new List<SceneryBlock>();
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
				blk = MapData.TerrainBlocks.find(pos);
				//if it doesn't exist
				if(blk == null) {
					//create a new one
					blk = new TerrainBlock(defaultBlockID, pos, DIRECTION.North);
					//and add it to the MapData
					MapData.TerrainBlocks.add(blk);
				}
				//link the block to the room
				Blocks.Add(blk);
				//and link the room to the block
				blk.Room = this;
				//If the block is along the edge, give it a wall prefab
				if(this.isEdge(blk.Position)) {
					blk.Wall = GameObjectResourcePool.getResource(defaultWall, blk.Position, getWallSide(blk.Position).toRotationVector());
				}
			}
		}
		//For each block in the room, reevaluate its neighbor links
		foreach(TerrainBlock blks in Blocks) {
			MapData.TerrainBlocks.relinkNeighbors(blks);
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
		foreach(TerrainBlock blk in Blocks) {
			//unlink the room from the block
			blk.Room = null;
			//if the room has a wall, return it to the resource pool
			if(blk.Wall != null) {
				string blkid = blk.Wall.GetComponent<SceneryMonoBehavior>().BlockID;
				GameObjectResourcePool.returnResource(blkid, blk.Wall);
			}
		}
		//remove all the links to terrain
		Blocks.Clear();
	}
	#endregion (un)linkTerrain

	public void linkRoomsViaDoors(){
		LinkedRooms.Clear();
		RoomPaths.Clear();
		foreach(SceneryBlock dr in Doors) {
			Vector3 checkPos = getDoorCheckPosition(dr);
			SceneryBlock scnBlk = null;
			scnBlk = MapData.SceneryBlocks.find(checkPos);
			if(scnBlk == null){
				continue;
			}
			if(!scnBlk.BlockInfo.isDoor) {
				continue;
			}
			if(scnBlk.Orientation != dr.Orientation.Opposite()){
				continue;
			}
			LinkedRooms.Add(dr, MapData.TheFarRooms.find(checkPos));
		}
		foreach(KeyValuePair<SceneryBlock, ARTFRoom> kvp1 in LinkedRooms){
			foreach(KeyValuePair<SceneryBlock, ARTFRoom> kvp2 in LinkedRooms){
				RoomPaths.Add(new KeyValuePair<ARTFRoom, ARTFRoom>(kvp1.Value, kvp2.Value),
				              Pathfinder.getSingleRoomPath(kvp1.Key.Position, kvp2.Key.Position));
			}
		}
	}

	public Vector3 getDoorCheckPosition(SceneryBlock dr){
		Vector3 checkPos = dr.Position;
		switch(dr.Orientation){
		case DIRECTION.North:
			checkPos.z += 1;
			break;
		case DIRECTION.South:
			checkPos.z -= 1;
			break;
		case DIRECTION.East:
			checkPos.x += 1;
			break;
		case DIRECTION.West:
			checkPos.x -= 1;
			break;
		}
		return checkPos;
	}

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
		foreach(TerrainBlock blk in Blocks) {
			blk.move(offset);
		}
		/* Should be unnecessary. Blocks are now only linked within rooms
		//for each block
		foreach(TerrainBlock blk in Blocks) {
			//if it is an edge block
			if(isEdge(blk.Position)) {
				//force it to re-identify its neighbors
				MapData.TerrainBlocks.relinkNeighbors(blk);
			}
		}*/
	}

	/*
	 * public void resize(Vector3 oldCor, Vector3 newCor)
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
		foreach(TerrainBlock blk in Blocks) {
			if(!inRoom(blk.Position)) {
				MapData.TerrainBlocks.remove(blk);
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
	public void remove() {
		foreach(TerrainBlock blk in this.Blocks) {
			MapData.TerrainBlocks.remove(blk);
		}
		this.Blocks.Clear();
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
