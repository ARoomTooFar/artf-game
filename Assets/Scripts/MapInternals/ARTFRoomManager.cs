using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARTFRoomManager {

	private List<ARTFRoom> roomList = new List<ARTFRoom>();

	public ARTFRoomManager() {
	}

	/*
	 * public bool addRoom(Vector3 pos1, Vector3 pos2)
	 * 
	 * Adds a room with two corners at the given positions
	 */
	public bool addRoom(Vector3 pos1, Vector3 pos2){
		//Creates a new room
		ARTFRoom nRoom = new ARTFRoom(pos1, pos2);
		//If an existing room intersects with the new room
		if(doAnyRoomsIntersect(nRoom)){
			//abort and return false
			return false;
		}
		//add the new room to the list of rooms
		roomList.Add(nRoom);
		//link the needed terrain to it
		nRoom.linkTerrain();
		return true;
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
		//if it doesn't exist abort
		if(rm == null) {
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
		ARTFRoom tempRoom = new ARTFRoom(room.LLCorner + offset, room.URCorner + offset);
		//check if the new room intersects
		return doAnyRoomsIntersect(tempRoom);
	}

	/*
	 * public bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2)
	 * 
	 * Checks to see if two given rooms intersect with each other
	 */
	public bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2){
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
	public ARTFRoom getRoom(Vector3 position){
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