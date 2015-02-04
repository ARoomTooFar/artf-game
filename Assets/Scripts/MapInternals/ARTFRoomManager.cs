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

	/*
	 * public bool addRoom(Vector3 pos1, Vector3 pos2)
	 * 
	 * Adds a room with two corners at the given positions
	 */
	public bool addRoom(Vector3 pos1, Vector3 pos2){
		//Creates a new room
		ARTFRoom nRoom = new ARTFRoom(pos1, pos2);
		return addRoom(nRoom);
	}

	/*
	 * public bool addRoom(ARTFRoom rm)
	 * 
	 * Adds a room
	 */
	public bool addRoom(ARTFRoom rm){
		//If an existing room intersects with the new room
		if(doAnyRoomsIntersect(rm)){
			//abort and return false
			return false;
		}
		//add the new room to the list of rooms
		roomList.Add(rm);
		//link the needed terrain to it
		rm.linkTerrain();
		return true;
	}

	public void removeRoom(Vector3 pos){
		ARTFRoom rm = getRoom(pos);
		removeRoom(rm);
	}

	public void removeRoom(ARTFRoom rm){
		rm.remove();
		this.roomList.Remove(rm);
	}

	/*
	 * public bool moveRoom(Vector3 position, Vector3 offset)
	 * 
	 * Moves the room at position by a value specified by offset
	 */
	public bool moveRoom(Vector3 position, Vector3 offset){
		//get the room at position
		ARTFRoom rm = getRoom(position);
		//if it doesn't exist, return false
		if(rm == null) {
			return false;
		}
		//if the move would cause an intersect, abort
		if(doAnyRoomsIntersect(rm, offset)){
			return false;
		}
		//tell the room to move
		return rm.Move(offset);
	}

	/*
	 * public bool resizeRoom(Vector3 corner, Vector3 nCorner)
	 * 
	 * Resizes the room at corner by moving corner to nCorner
	 */
	public bool resizeRoom(Vector3 corner, Vector3 nCorner){
		//get the room at corner
		ARTFRoom rm = getRoom(corner);
		//if corner is not a corner of the room, abort
		if(!rm.isCorner(corner)) {
			return false;
		}
		//if it doesn't exist, abort
		if(rm == null) {
			return false;
		}
		//if the resize would cause an intersect, abort
		if(doAnyRoomsIntersect(rm, corner, nCorner)){
			return false;
		}
		//tell the room to resize itself
		return rm.Resize(corner, nCorner);
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

	/*
	 * public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 offset)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list if it is moved by offset.
	 */
	public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 offset){
		//get a new room in the offset position
		room.Move(offset);
		//check if the new room intersects
		bool retVal = doAnyRoomsIntersect(room);
		room.Move(-offset);
		return retVal;
	}

	/*
	 * public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 corner, Vector3 nCorner)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list if it is resized
	 */
	public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 corner, Vector3 nCorner){
		//get a new room in the offset position
		room.Resize(corner, nCorner);
		//check if the new room intersects
		bool retVal = doAnyRoomsIntersect(room);
		room.Resize(nCorner, corner);
		return retVal;
	}

	/*
	 * public bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2)
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
	 * public ARTFRoom getRoom(Vector3 position)
	 * 
	 * Gets the room at a given position
	 */
	protected internal ARTFRoom getRoom(Vector3 position){
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