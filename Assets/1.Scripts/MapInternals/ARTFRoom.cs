using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Object to represent a room within the mapdata
 */
public partial class ARTFRoom : Square {

	#region PrivateVariables
	protected string floorType = "{0}/Floors/IndustrialFloor1";
	protected string roomCornerId = "LevelEditor/Other/RoomCorner";
	protected string wallType = "{0}/Other/wallstoneend";
	#endregion PrivateVariables

	#region Properties
	public bool placedThisSession = true;

	public List<SceneryBlock> Walls { get; private set; }

	public Dictionary<DIRECTION, List<GameObject>> StretchWalls { get; private set; }

	public List<SceneryBlock> Scenery { get; private set; }

	public List<MonsterBlock> Monster { get; private set; }

	public GameObject Floor { get; private set; }

	//A list of the other rooms this room is linked to
	public Dictionary<SceneryBlock, ARTFRoom> LinkedRooms { get; private set; }

	//A list of doors within the room
	public List<SceneryBlock> Doors { get; private set; }

	public int CurrentPoints { get; private set; }

	public string PointString{ get { return String.Format("{0}/{1}", CurrentPoints, Points); } }
	
	//Stored paths to get from point A to point B. Primarily for storing paths from one door to another
	public Dictionary<KeyValuePair<Vector3, Vector3>, List<Vector3>> RoomPaths { get; private set; }

	#region Corners	
	public List<GameObject> CornerMarkers { get; protected set;}

	public virtual void updateMarkerPositions() {
		for(int i = 0; i < 4; ++i) {
			CornerMarkers[i].transform.position = Corners[i];
		}
	}

	public virtual void setMarkerActive(bool active) {
		foreach(GameObject cor in CornerMarkers) {
			cor.SetActive(active);
		}
	}
	#endregion Corners

	public string SaveString {
		get{ return LLCorner.toCSV() + "," + URCorner.toCSV();}
	}
	#endregion Properties

	/* 	
	 * Constructor
	 */
	public ARTFRoom(Vector3 pos1, Vector3 pos2,
	                string floor = "{0}/Floors/IndustrialFloor1",
	                string wall = "{0}/Other/wallstoneend") : base(pos1, pos2) {
		floorType = floor;
		wallType = wall;
		this.Floor = GameObjectResourcePool.getResource(floorType, this.LLCorner, Vector3.zero);
		setFloor();
		this.LinkedRooms = new Dictionary<SceneryBlock, ARTFRoom>();
		this.Scenery = new List<SceneryBlock>();
		this.Monster = new List<MonsterBlock>();
		this.Doors = new List<SceneryBlock>();
		this.Walls = new List<SceneryBlock>();
		this.StretchWalls = new Dictionary<DIRECTION, List<GameObject>>(){
			{DIRECTION.North, new List<GameObject>()},
			{DIRECTION.South, new List<GameObject>()},
			{DIRECTION.East, new List<GameObject>()},
			{DIRECTION.West, new List<GameObject>()},
		};
		//setWalls();
		setAllStretchWalls();

		this.RoomPaths = new Dictionary<KeyValuePair<Vector3, Vector3>, List<Vector3>>();
		this.CornerMarkers = new List<GameObject>();

		GameObject marker;
		if(Global.inLevelEditor) {
			foreach(Vector3 cor in Corners) {
				marker = GameObjectResourcePool.getResource(roomCornerId, cor, Vector3.zero);
				CornerMarkers.Add(marker);
				marker.SetActive(Mode.isRoomMode());
			}
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
			if(!scnBlk.SceneryBlockInfo.isDoor) {
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
				if(kvp1.Equals(kvp2)) {
					continue;
				}
				//find the path between the two and store it
				List<Vector3> path = Pathfinder.getSingleRoomPath(kvp1.Key.Position, kvp2.Key.Position);
				if(path == null) {
					continue;
				}
				RoomPaths.Add(new KeyValuePair<Vector3, Vector3>(kvp1.Key.Position, kvp2.Key.Position), path);
				
			}
		}
	} 

	#region Monsters
	
	public void addMonster(MonsterBlock mon) {
		CurrentPoints += mon.MonsterBlockInfo.basePoints;
		Monster.Add(mon);
	}
	
	public void removeMonster(MonsterBlock mon) {
		CurrentPoints -= mon.MonsterBlockInfo.basePoints;
		Monster.Remove(mon);
	}
	#endregion Monsters


	#region ManipulationFunctions

	public void setFloor() {
		this.Floor.transform.position = new Vector3(this.Center.x, -.03f, this.Center.z);
		this.Floor.transform.localScale = new Vector3(this.Length, 1, this.Height);
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
		foreach(SceneryBlock blk in Scenery) {
			blk.move(offset);
		}
		foreach(MonsterBlock blk in Monster) {
			blk.move(offset);
		}
		foreach(SceneryBlock wall in Walls) {
			wall.move(offset);
		}
		foreach(SceneryBlock dr in (new List<SceneryBlock>(Doors))) {
			MapData.SceneryBlocks.remove(dr);
		}
		setFloor();
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
	
		List<SceneryBlock> remScn = new List<SceneryBlock>();
		foreach(SceneryBlock door in Doors) {
			if(!isEdge(door.Position)) {
				remScn.Add(door);
			}
		}
		foreach(SceneryBlock door in remScn) {
			Doors.Remove(door);
			MapData.SceneryBlocks.remove(door);
		}
		remScn.Clear();
		foreach(SceneryBlock scn in Scenery) {
			if(!inRoom(scn.Position)) {
				remScn.Add(scn);
			}
		}
		foreach(SceneryBlock scn in remScn) {
			Scenery.Remove(scn);
			MapData.SceneryBlocks.remove(scn);
		}
		List<MonsterBlock> remMon = new List<MonsterBlock>();
		foreach(MonsterBlock mon in Monster) {
			if(!inRoom(mon.Position)) {
				remMon.Add(mon);
			}
		}
		foreach(MonsterBlock mon in remMon) {
			Monster.Remove(mon);
			MapData.MonsterBlocks.remove(mon);
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
		Scenery.Clear();
		foreach(MonsterBlock mon in Monster) {
			mon.remove();
		}
		Monster.Clear();
		foreach(SceneryBlock wall in Walls) {
			wall.remove();
		}
		Walls.Clear();
		foreach(List<GameObject> walls in StretchWalls.Values) {
			foreach(GameObject wall in walls){
				GameObjectResourcePool.returnResource("{0}/Other/wallstoneend", wall);
			}
		}
		Walls.Clear();
		GameObjectResourcePool.returnResource(floorType, Floor);
		foreach(GameObject cor in CornerMarkers) {
			GameObjectResourcePool.returnResource(roomCornerId, cor);
		}
	}
	
	public void setWalls() {
		foreach(SceneryBlock wall in Walls) {
			wall.remove();
		}
		Walls.Clear();
		Vector3 vec;
		for(float i = LLCorner.x; i <= URCorner.x; ++i) {
			for(float j = LLCorner.z; j <= URCorner.z; ++j) {
				vec = new Vector3(i, 0, j);
				if(!isEdge(vec)) {
					continue;
				}

//				if(Global.inLevelEditor)
					Walls.Add(new SceneryBlock(wallType, vec, DIRECTION.North));
			}
		}
	}

	public void setAllStretchWalls(){
		setStretchWalls(DIRECTION.North);
		setStretchWalls(DIRECTION.South);
		setStretchWalls(DIRECTION.West);
		setStretchWalls(DIRECTION.East);
	}

	public void setStretchWalls(DIRECTION dir){
		foreach(GameObject wall in StretchWalls[dir]) {
			GameObjectResourcePool.returnResource("{0}/Other/StretchWall", wall);
		}
		StretchWalls[dir].Clear();

		bool isNS = dir == DIRECTION.North || dir == DIRECTION.South;
		int staticPos = 0;
		switch(dir) {
		case DIRECTION.North:
			staticPos = Mathf.RoundToInt(URCorner.z);
			break;
		case DIRECTION.South:
			staticPos = Mathf.RoundToInt(LLCorner.z);
			break;
		case DIRECTION.East:
			staticPos = Mathf.RoundToInt(URCorner.x);
			break;
		case DIRECTION.West:
			staticPos = Mathf.RoundToInt(LLCorner.x);
			break;
		}

		List<int> coords = new List<int>();

		coords.Add(Mathf.RoundToInt(isNS ? LLCorner.x - 2 : LLCorner.z - 2));
		coords.Add(Mathf.RoundToInt(isNS ? URCorner.x + 2 : URCorner.z + 2));

		foreach(SceneryBlock dr in Doors) {
			if(this.getWallSide(dr.Position) == dir){
				coords.Add(Mathf.RoundToInt(isNS ? dr.Position.x : dr.Position.z));
			}
		}

		coords.Sort();

		GameObject nWall;
		Vector3 nPos;
		float varPos = 0;
		Vector3 scale;
		for(int i = 1; i < coords.Count; ++i) {
			varPos = (coords[i-1] + coords[i])/2.0f;
			nPos = new Vector3(isNS? varPos: staticPos, 0, !isNS? varPos: staticPos);
			nWall = GameObjectResourcePool.getResource("{0}/Other/StretchWall", nPos, dir.toRotationVector());
			scale = nWall.transform.localScale;
			scale.x = (coords[i] - 2) - (coords[i-1]+2) + 1;
			nWall.transform.localScale = scale;
			StretchWalls[dir].Add(nWall);
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
	public bool isNearEdge(Vector3 pos, float thresh) {
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
	public Vector3 nearEdgePosition(Vector3 pos) {
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
		return inSquare(pos);
	}

	public bool isWalkable(Vector3 pos) {
		if(!inRoom(pos)) {
			return false;
		}
		foreach(SceneryBlock blk in Scenery) {
			if(blk.Coordinates.Contains(pos)) {
				return blk.SceneryBlockInfo.Walkable;
			}
		}
		return true;
	}

	public bool isPathable(Vector3 pos) {
		if(!inRoom(pos)) {
			return false;
		}
		foreach(SceneryBlock blk in Scenery) {
			if(blk.Coordinates.Contains(pos)) {
				return blk.SceneryBlockInfo.Pathable;
			}
		}
		return true;
	}
	#endregion PositionChecks
}
