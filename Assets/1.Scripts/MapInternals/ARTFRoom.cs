using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a room within the mapdata
 */
public partial class ARTFRoom {

	#region PrivateVariables
	private static string defaultBlockID = "LevelEditor/Rooms/floortile";
	private static string defaultFloor = "{0}/Floors/IndustrialFloor1";
	#endregion PrivateVariables

	#region Properties
	//A list of the TerrainBlocks contained within the room
	public List<TerrainBlock> Blocks {
		get;
		private set;
	}

	public GameObject Floor {
		get;
		private set;
	}

	//A list of the other rooms this room is linked to
	public Dictionary<SceneryBlock, ARTFRoom> LinkedRooms {
		get;
		private set;
	}

	//A list of doors within the room
	public List<SceneryBlock> Doors {
		get;
		private set;
	}

	//Stored paths to get from point A to point B. Primarily for storing paths from one door to another
	public Dictionary<KeyValuePair<Vector3, Vector3>, List<Vector3>> RoomPaths {
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

	public GameObject LLMarker { get; protected set; }
	public GameObject URMarker { get; protected set; }
	public GameObject LRMarker { get; protected set; }
	public GameObject ULMarker { get; protected set; }



	public virtual void updateMarkerPositions(){
		LLMarker.transform.root.position = LLCorner;
		URMarker.transform.root.position = URCorner;
		LRMarker.transform.root.position = LRCorner;
		ULMarker.transform.root.position = ULCorner;
	}

	public virtual void setMarkerActive(bool active){
		LLMarker.SetActive (active);
		URMarker.SetActive (active);
		LRMarker.SetActive (active);
		ULMarker.SetActive (active);
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

		this.Floor = GameObjectResourcePool.getResource(defaultFloor, this.LLCorner, Vector3.zero);
		setFloor();

		this.LinkedRooms = new Dictionary<SceneryBlock, ARTFRoom>();
		this.Doors = new List<SceneryBlock>();
		this.RoomPaths = new Dictionary<KeyValuePair<Vector3, Vector3>, List<Vector3>>();
		if(Global.inLevelEditor) {
			this.URMarker = GameObjectResourcePool.getResource("Prefabs/RoomCorner", URCorner, Vector3.zero);
			this.LLMarker = GameObjectResourcePool.getResource("Prefabs/RoomCorner", LLCorner, Vector3.zero);
			this.ULMarker = GameObjectResourcePool.getResource("Prefabs/RoomCorner", ULCorner, Vector3.zero);
			this.LRMarker = GameObjectResourcePool.getResource("Prefabs/RoomCorner", LRCorner, Vector3.zero);
			setMarkerActive(Mode.isRoomMode());
		}
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
					blk.addWall(getWallSide(blk.Position));
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
				blk.removeWall();
			}
		}
		//remove all the links to terrain
		Blocks.Clear();
	}
	#endregion (un)linkTerrain

	/*
	 * public void linkRoomsViaDoors()
	 * 
	 * uses Doors to create links to other rooms
	 */
	public void linkRoomsViaDoors() {
		//clear the currently known links
		LinkedRooms.Clear();
		//for each door
		foreach(SceneryBlock dr in Doors) {
			//get the position where a linked door needs to be
			Vector3 checkPos = dr.doorCheckPosition;
			//grab the SceneryBlock at this position if it exists
			SceneryBlock scnBlk = MapData.SceneryBlocks.find(checkPos);
			//if there is no scenery block, move on
			if(scnBlk == null) {
				continue;
			}
			//if the block is not a door, move on
			if(!scnBlk.BlockInfo.isDoor) {
				continue;
			}
			//if the door is not facing the current door, move on
			if(scnBlk.Orientation != dr.Orientation.Opposite()) {
				continue;
			}
			//make the link
			LinkedRooms.Add(dr, MapData.TheFarRooms.find(checkPos));
		}
		//after links are made, get the paths between them all
		createRoomPaths();
	}

	/*
	 * public void createRoomPaths()
	 * 
	 * gets the path between each door in the room
	 */
	public void createRoomPaths() {
		//clear the currently stored path
		RoomPaths.Clear();
		//for each pair of doors
		foreach(KeyValuePair<SceneryBlock, ARTFRoom> kvp1 in LinkedRooms) {
			foreach(KeyValuePair<SceneryBlock, ARTFRoom> kvp2 in LinkedRooms) {
				if(kvp1.Key == kvp2.Key){
					continue;
				}
				//find the path between the two and store it
				List<Vector3> path = Pathfinder.getSingleRoomPath(kvp1.Key.Position, kvp2.Key.Position);
				path.Insert(0, kvp1.Key.doorCheckPosition);
				path.Insert(path.Count, kvp2.Key.doorCheckPosition);
				RoomPaths.Add(new KeyValuePair<Vector3, Vector3>(kvp1.Key.Position, kvp2.Key.Position),
				              path);
			}
		}
	} 

	#region ManipulationFunctions

	public void setFloor(){
		Vector3 p = this.Center;
		this.Floor.transform.position = new Vector3(p.x, -.01f, p.z);
		Vector3 scale = this.Floor.transform.localScale;
		scale.x = this.Length;
		scale.z = this.Height;
		this.Floor.transform.localScale = scale;
	}


	/*
	 * public void move(Vector3 offset)
	 * 
	 * Moves a room in the x/y/z direction specified
	 * by the offset Vector3
	 */
	public void move(Vector3 offset) {
		List<ARTFRoom> rmlst = new List<ARTFRoom>();
		foreach(SceneryBlock dr in this.Doors) {
			rmlst.Add(MapData.TheFarRooms.find(dr.doorCheckPosition));
		}
		//Shift the LowerLeft and UpperRight corners by offset
		LLCorner = LLCorner + offset;
		URCorner = URCorner + offset;
		//move each block by offset
		foreach(TerrainBlock blk in Blocks) {
			blk.move(offset);
		}
		setFloor();
		updateMarkerPositions();
		foreach(ARTFRoom rm in rmlst) {
			rm.linkRoomsViaDoors();
		}
		this.linkRoomsViaDoors();
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
		List<ARTFRoom> rmlst = new List<ARTFRoom>();
		foreach(SceneryBlock dr in this.Doors) {
			rmlst.Add(MapData.TheFarRooms.find(dr.doorCheckPosition));
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
		List<SceneryBlock> remDoor = new List<SceneryBlock> ();
		foreach (SceneryBlock door in Doors) {
			if(!isEdge(door.Position)){
				remDoor.Add(door);
			}
		}
		foreach (SceneryBlock door in remDoor) {
			Doors.Remove(door);
			MapData.SceneryBlocks.remove(door);
		}
		//relink blocks to this room
		linkTerrain();

		setFloor();


		updateMarkerPositions();
		foreach(ARTFRoom rm in rmlst) {
			rm.linkRoomsViaDoors();
		}
		this.linkRoomsViaDoors();
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
		GameObjectResourcePool.returnResource(defaultFloor, Floor);
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
	 * check if we're close to the edge of a room
	 */ 
	public bool isCloseToEdge(Vector3 pos, float thresh){
		if(Math.Abs(pos.z - LLCorner.z) <= thresh || Math.Abs(URCorner.z - pos.z) <= thresh) {
			return true;
		}

		if(Math.Abs(pos.x - LLCorner.x) <= thresh || Math.Abs(URCorner.x - pos.x) <= thresh) {
			return true;
		}

		return false;
	}


	/*
	 * return edge the mouse is nearest
	 */ 
	public Vector3 getNearestEdgePosition(Vector3 pos){
		float zDistLL;
		float xDistLL;
		float zDistUR;
		float xDistUR;

		Vector3 returnPos = new Vector3();

		zDistLL = Mathf.Abs(pos.z - LLCorner.z);
		xDistLL = Mathf.Abs(pos.x - LLCorner.x);
		zDistUR = Mathf.Abs(pos.z - URCorner.z);
		xDistUR = Mathf.Abs(pos.x - URCorner.x);

		bool zURSmallest = false;
		if(zDistUR < zDistLL){
			zURSmallest = true;
		}

		bool xURSmallest = false;
		if(xDistUR < xDistLL){
			xURSmallest = true;
		}


		if(zURSmallest && xURSmallest){
			if(zDistUR < xDistUR){
				returnPos = new Vector3(pos.x, 0f, URCorner.z);
			}else{
				returnPos = new Vector3(URCorner.x, 0f, pos.z);
			}
		}else if (zURSmallest && !xURSmallest){
			if(zDistUR < xDistLL){
				returnPos = new Vector3(pos.x, 0f, URCorner.z);
			}else{
				returnPos = new Vector3(LLCorner.x, 0f, pos.z);
			}
		}else if (!zURSmallest && !xURSmallest){
			if(zDistLL < xDistLL){
				returnPos = new Vector3(pos.x, 0f, LLCorner.z);
			}else{
				returnPos = new Vector3(LLCorner.x, 0f, pos.z);
			}
		}else if (!zURSmallest && xURSmallest){
			if(zDistLL < xDistUR){
				returnPos = new Vector3(pos.x, 0f, LLCorner.z);
			}else{
				returnPos = new Vector3(URCorner.x, 0f, pos.z);
			}
		}

//		Debug.Log (returnPos);
		return (returnPos);
	}

	/*
	 * public DIRECTION getWallSide(Vector3 pos)
	 * 
	 * Returns a direction corresponding to the which wall the position is in 
	 * 
	 */
	public DIRECTION getWallSide(Vector3 pos) {
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
