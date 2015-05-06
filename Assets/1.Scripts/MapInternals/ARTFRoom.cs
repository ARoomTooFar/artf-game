using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a room within the mapdata
 */
public partial class ARTFRoom : Square {

	#region PrivateVariables
	private static string defaultFloor = "{0}/Floors/IndustrialFloor1";
	private static string roomCornerId = "LevelEditor/Other/RoomCorner";
	private static string wallType = "{0}/Rooms/wallstoneend";
	#endregion PrivateVariables

	#region Properties
	public bool placedThisSession = true;

	public List<SceneryBlock> Walls {
		get;
		private set;
	}

	public List<SceneryBlock> Scenery {
		get;
		private set;
	}

	public List<MonsterBlock> Monster {
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

	public int CurrentPoints {
		get;
		private set;
	}


	//Stored paths to get from point A to point B. Primarily for storing paths from one door to another
	public Dictionary<KeyValuePair<Vector3, Vector3>, List<Vector3>> RoomPaths {
		get;
		private set;
	}

	#region Corners
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

	public string SaveString {
		get{ return LLCorner.toCSV() + "," + URCorner.toCSV();}
	}
	#endregion Properties
	
	/*
	 * Constructor
	 */
	public ARTFRoom(Vector3 pos1, Vector3 pos2) : base(pos1, pos2) {
		this.Floor = GameObjectResourcePool.getResource(defaultFloor, this.LLCorner, Vector3.zero);
		setFloor();
		this.LinkedRooms = new Dictionary<SceneryBlock, ARTFRoom>();
		this.Scenery = new List<SceneryBlock> ();
		this.Walls = new List<SceneryBlock>();
		setWalls();
		this.Monster = new List<MonsterBlock> ();
		this.Doors = new List<SceneryBlock>();
		this.RoomPaths = new Dictionary<KeyValuePair<Vector3, Vector3>, List<Vector3>>();
		if(Global.inLevelEditor) {
			this.URMarker = GameObjectResourcePool.getResource(roomCornerId, URCorner, Vector3.zero);
			this.LLMarker = GameObjectResourcePool.getResource(roomCornerId, LLCorner, Vector3.zero);
			this.ULMarker = GameObjectResourcePool.getResource(roomCornerId, ULCorner, Vector3.zero);
			this.LRMarker = GameObjectResourcePool.getResource(roomCornerId, LRCorner, Vector3.zero);
			setMarkerActive(Mode.isRoomMode());
		}
	}

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
				if(kvp1.Equals(kvp2)){
					continue;
				}
				//find the path between the two and store it
				List<Vector3> path = Pathfinder.getSingleRoomPath(kvp1.Key.Position, kvp2.Key.Position);
				if(path == null){
					continue;
				}
				try{
				RoomPaths.Add(new KeyValuePair<Vector3, Vector3>(kvp1.Key.Position, kvp2.Key.Position),
					              path);} catch{}
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
	public override void move(Vector3 offset) {
		base.move(offset);
		//move each block by offset
		foreach (SceneryBlock blk in Scenery) {
			blk.move (offset);
		}
		foreach(MonsterBlock blk in Monster) {
			blk.move(offset);
		}
		setFloor();
		foreach(SceneryBlock wall in Walls){
			wall.move(offset);
		}
		updateMarkerPositions();
	}

	/*
	 * public void resize(Vector3 oldCor, Vector3 newCor)
	 * 
	 * Resizes a room by moving one corner to a new position
	 */
	public override void resize(Vector3 oldCor, Vector3 newCor) {
		//Make sure that the old corner is actually a corner
		if(!isCorner(oldCor)) {
			return;
		}
		base.resize(oldCor, newCor);
	
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

		setFloor();
		setWalls();
		updateMarkerPositions();
	}

	/*
	 * public void remove()
	 * 
	 * Removes all linked blocks from MapData
	 */
	public void remove() {
		foreach(SceneryBlock scn in Scenery) {
			scn.remove();
		}
		foreach(MonsterBlock mon in Monster) {
			mon.remove();
		}
		foreach(SceneryBlock wall in Walls) {
			wall.remove();
		}
		GameObjectResourcePool.returnResource(defaultFloor, Floor);
		if(LLMarker != null) {
			GameObjectResourcePool.returnResource(roomCornerId, LLMarker);
		}
		if(URMarker != null) {
			GameObjectResourcePool.returnResource(roomCornerId, URMarker);
		}
		if(ULMarker != null) {
			GameObjectResourcePool.returnResource(roomCornerId, ULMarker);
		}
		if(LRMarker != null) {
			GameObjectResourcePool.returnResource(roomCornerId, LRMarker);
		}
	}
	#endregion ManipulationFunctions

	#region PositionChecks
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
	public bool isNearEdge(Vector3 pos, float thresh){
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
	public Vector3 nearEdgePosition(Vector3 pos){
		float leftXDist = Mathf.Abs(pos.x - LLCorner.x);
		float rightXDist = Mathf.Abs(pos.x - URCorner.x);
		float leftZDist = Mathf.Abs(pos.z - LLCorner.z);
		float rightZDist = Mathf.Abs(pos.z - URCorner.z);

		float x = leftXDist < rightXDist ? LLCorner.x : URCorner.x;
		float z = leftZDist < rightZDist ? LLCorner.z : URCorner.z;

		return Mathf.Min(leftXDist, rightXDist) < Mathf.Min(leftZDist, rightZDist) ? new Vector3(x, 0f, pos.z) : new Vector3(pos.x, 0f, z);
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

	public bool isWalkable(Vector3 pos){
		if(!inRoom(pos)) {
			return false;
		}
		foreach(SceneryBlock blk in Scenery) {
			if(blk.Coordinates.Contains(pos)){
				return blk.Walkable;
			}
		}
		return true;
	}

	public bool isPathable(Vector3 pos){
		if(!inRoom(pos)) {
			return false;
		}
		foreach(SceneryBlock blk in Scenery) {
			if(blk.Coordinates.Contains(pos)){
				return blk.Pathable;
			}
		}
		return true;
	}

	public void addMonster(MonsterBlock mon){
		CurrentPoints += mon.BlockInfo.Points;
		Monster.Add(mon);
	}

	public void removeMonster(MonsterBlock mon){
		CurrentPoints -= mon.BlockInfo.Points;
		Monster.Remove(mon);
	}

	public void setWalls(){
		foreach(SceneryBlock wall in Walls) {
			wall.remove();
		}
		Walls.Clear();
		Vector3 vec;
		for(float i = LLCorner.x; i <= URCorner.x; ++i) {
			for(float j = LLCorner.z; j <= URCorner.z; ++j){
				vec = new Vector3(i,0,j);
				if(!isEdge(vec)){
					continue;
				}
				Walls.Add(new SceneryBlock(wallType, vec, DIRECTION.North));
			}
		}
	}

	#endregion PositionChecks
}
