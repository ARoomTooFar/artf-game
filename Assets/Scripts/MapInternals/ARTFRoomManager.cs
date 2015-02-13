using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARTFRoomManager {

	protected internal List<ARTFRoom> roomList = new List<ARTFRoom>();

	public ARTFRoomManager() {
	}

	//Shallow Copy Constructor. Testing only
	protected internal ARTFRoomManager(ARTFRoomManager rm){
		this.roomList = rm.roomList;
	}

	#region ManipulationFunctions

	#region Add
	/*
	 * public void add(Vector3 pos1, Vector3 pos2)
	 * 
	 * Adds a room with two corners at the given positions
	 */
	public void add(Vector3 pos1, Vector3 pos2){
		add(new ARTFRoom(pos1, pos2));
	}

	/*
	 * public void add(ARTFRoom rm)
	 * 
	 * Adds a room
	 */
	public void add(ARTFRoom rm){
		//add the new room to the list of rooms
		roomList.Add(rm);
		//link the needed terrain to it
		rm.linkTerrain();
	}
	#endregion Add

	#region Remove
	/*
	 * public void remove(Vector3 pos)
	 * 
	 * Removes a room at the given position from the stored data
	 */
	public void remove(Vector3 pos){
		remove(find(pos));
	}

	/*
	 * public void remove(ARTFRoom rm)
	 * 
	 * Removes the specified room from the stored data
	 */
	public void remove(ARTFRoom rm){
		rm.remove();
		roomList.Remove(rm);
	}
	#endregion Remove

	#region Move
	/*
	 * public void move(Vector3 position, Vector3 offset)
	 * 
	 * Moves the room at position by a value specified by offset
	 */
	public void move(Vector3 position, Vector3 offset){
		move(find(position), offset);
	}

	/*
	 * public void move(Vector3 position, Vector3 offset)
	 * 
	 * Moves the room at position by a value specified by offset
	 */
	public void move(ARTFRoom rm, Vector3 offset){
		rm.move(offset);
	}
	#endregion Move

	/*
	 * public bool resize(Vector3 corner, Vector3 nCorner)
	 * 
	 * Resizes the room at corner by moving corner to nCorner
	 */
	public void resize(Vector3 corner, Vector3 nCorner){
		//get the room at corner
		ARTFRoom rm = find(corner);
		//if it doesn't exist, abort
		if(rm == null) {
			return;
		}
		//if corner is not a corner of the room, abort
		if(!rm.isCorner(corner)) {
			return;
		}
		//tell the room to resize itself
		rm.resize(corner, nCorner);
	}

	#endregion ManipulationFunctions

	#region ValidationFunctions

	#region Intersection
	/*
	 * public static bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2)
	 * 
	 * Checks to see if two given rooms intersect with each other
	 */
	public static bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2){
		//for each corner in room1
		foreach(Vector3 corn in rm1.Corners) {
			//if that corner is inside room2
			if(rm2.inRoom(corn)){
				return true;
			}
		}
		//for each corner in room2
		foreach(Vector3 corn in rm2.Corners) {
			//if that corner is inside room1
			if(rm1.inRoom(corn)){
				return true;
			}
		}
		return false;
	}

	/*
	 * public bool doAnyRoomsIntersect(ARTFRoom room)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list.
	 */
	public bool doAnyRoomsIntersect(ARTFRoom room){
		//for each extant room
		foreach(ARTFRoom rm in roomList) {
			//if the room is the room we're checking, move on
			if(rm.Equals(room)) continue;

			//if the rooms intersect
			if(doRoomsIntersect(rm, room)){
				return true;
			}
		}
		return false;
	}
	#endregion Intersection

	#region Move
	public bool isMoveValid(Vector3 oldPos, Vector3 newPos){
		return isMoveValid(find(oldPos), newPos - oldPos);
	}

	/*
	 * public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 offset)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list if it is moved by offset.
	 */
	public bool isMoveValid(ARTFRoom room, Vector3 offset){
		//get a new room in the offset position
		room.move(offset);
		//check if the new room intersects
		bool retVal = doAnyRoomsIntersect(room);
		room.move(-offset);
		return retVal;
	}
	#endregion Move

	#region Resize
	public bool isResizeValid(Vector3 oldCorner, Vector3 newCorner){
		return isResizeValid(find(oldCorner), oldCorner, newCorner);
	}

	/*
	 * public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 corner, Vector3 nCorner)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list if it is resized
	 */
	public bool isResizeValid(ARTFRoom room, Vector3 corner, Vector3 nCorner){
		//get a new room in the offset position
		room.resize(corner, nCorner);
		//check if the new room intersects
		bool retVal = doAnyRoomsIntersect(room);
		room.resize(nCorner, corner);
		return retVal;
	}
	#endregion Resize

	#endregion Validation

	/*
	 * public ARTFRoom getRoom(Vector3 position)
	 * 
	 * Gets the room at a given position
	 */
	protected internal ARTFRoom find(Vector3 position){
		//for each extant room
		foreach(ARTFRoom rm in roomList) {
			//if the position is
			if(rm.inRoom(position)){
				return rm;
			}
		}
		return null;
	}

}