using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARTFRoomManager {

	protected internal List<ARTFRoom> roomList = new List<ARTFRoom>();

	public ARTFRoomManager() {
	}

	//Shallow Copy Constructor. Testing only
	protected internal ARTFRoomManager(ARTFRoomManager rmMan) {
		this.roomList = rmMan.roomList;
	}
	
	public void clear() {
		foreach(ARTFRoom rm in roomList) {
			rm.remove();
		}
		roomList.Clear();
	}

	public string SaveString {
		get {
			string retVal = "rooms:";
			foreach(ARTFRoom rm in roomList) {
				retVal += rm.SaveString + " ";
			}
			return retVal + "\n";
		}
	}

	#region ManipulationFunctions
	#region Add
	/*
	 * public void add(Vector3 pos1, Vector3 pos2)
	 * 
	 * Adds a room with two corners at the given positions
	 */
	public void add(Vector3 pos1, Vector3 pos2, bool startRoom, bool endRoom) {
		add(new ARTFRoom(pos1, pos2, startRoom, endRoom));
	}

	/*
	 * public void add(ARTFRoom rm)
	 * 
	 * Adds a room
	 */
	public void add(ARTFRoom rm) {
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
	public void remove(Vector3 pos) {
		remove(find(pos));
	}

	/*
	 * public void remove(ARTFRoom rm)
	 * 
	 * Removes the specified room from the stored data
	 */
	public void remove(ARTFRoom rm) {
		rm.remove();
		roomList.Remove(rm);
	}
	#endregion Remove

	#region Move
	/*
	 * public void move(Vector3 pos, Vector3 offset)
	 * 
	 * Moves the room at position by a value specified by offset
	 */
	public void move(Vector3 pos, Vector3 offset) {
		move(find(pos), offset);
	}

	/*
	 * public void move(ARTFRoom rm, Vector3 offset)
	 * 
	 * Moves the room by a value specified by offset
	 */
	public void move(ARTFRoom rm, Vector3 offset) {
		rm.move(offset);
	}
	#endregion Move

	/*
	 * public bool resize(Vector3 oldCor, Vector3 newCor)
	 * 
	 * Resizes the room at oldCor by moving oldCor to newCor
	 */
	public void resize(Vector3 oldCor, Vector3 newCor) {
		//get the room at corner
		ARTFRoom rm = find(oldCor);
		//if it doesn't exist, abort
		if(rm == null) {
			return;
		}
		//if corner is not a corner of the room, abort
		if(!rm.isCorner(oldCor)) {
			return;
		}
		//tell the room to resize itself
		rm.resize(oldCor, newCor);
	}
	#endregion ManipulationFunctions

	#region ValidationFunctions
	#region Intersection
	/*
	 * public static bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2)
	 * 
	 * Checks to see if two given rooms intersect with each other
	 */
	public static bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2) {
		//for each corner in room1
		foreach(Vector3 cor in rm1.Corners) {
			//if that corner is inside room2
			if(rm2.inRoom(cor)) {
				return true;
			}
		}
		//for each corner in room2
		foreach(Vector3 cor in rm2.Corners) {
			//if that corner is inside room1
			if(rm1.inRoom(cor)) {
				return true;
			}
		}
		return false;
	}

	/*
	 * public bool doAnyRoomsIntersect(ARTFRoom rm)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list.
	 */
	public bool doAnyRoomsIntersect(ARTFRoom rm) {
		//for each extant room
		foreach(ARTFRoom other in roomList) {
			//if the room is the room we're checking, move on
			if(other.Equals(rm))
				continue;

			//if the rooms intersect
			if(doRoomsIntersect(other, rm)) {
				return true;
			}
		}
		return false;
	}
	#endregion Intersection

	#region Move
	/*
	 * public bool isMoveValid(Vector3 oldPos, Vector3 newPos)
	 * 
	 * Checks to see if a room at oldPos intersects with
	 * any rooms already in the list if it is moved by offset.
	 */
	public bool isMoveValid(Vector3 oldPos, Vector3 newPos) {
		return isMoveValid(find(oldPos), newPos - oldPos);
	}

	/*
	 * public bool isMoveValid(ARTFRoom rm, Vector3 offset)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list if it is moved by offset.
	 */
	public bool isMoveValid(ARTFRoom rm, Vector3 offset) {
		//get a new room in the offset position
		rm.move(offset);
		//check if the new room intersects
		bool retVal = doAnyRoomsIntersect(rm);
		rm.move(-offset);
		return retVal;
	}
	#endregion Move

	#region Resize
	/*
	 * public bool isResizeValid(Vector3 oldCor, Vector3 newCor)
	 * 
	 * Checks to see if a room at oldCor intersects with
	 * any rooms already in the list if it is resized
	 */
	public bool isResizeValid(Vector3 oldCor, Vector3 newCor) {
		return isResizeValid(find(oldCor), oldCor, newCor);
	}

	/*
	 * public bool isResizeValid(ARTFRoom rm, Vector3 oldCor, Vector3 newCor)
	 * 
	 * Checks to see if a given room intersects with
	 * any rooms already in the list if it is resized
	 */
	public bool isResizeValid(ARTFRoom rm, Vector3 oldCor, Vector3 newCor) {
		if (rm == null) {
			return false;
		}
		Square testSquare = new Square (rm.LLCorner, rm.URCorner);
		testSquare.resize (oldCor, newCor);
		if (testSquare.LLCorner.x >= testSquare.URCorner.x) {
			return false;
		}
		if (testSquare.LLCorner.z >= testSquare.URCorner.z) {
			return false;
		}
		Square roomSquare;
		foreach (ARTFRoom room in roomList) {
			if(rm.LLCorner == room.LLCorner){
				continue;
			}
			roomSquare = new Square(room.LLCorner, room.URCorner);
			if(testSquare.Intersect(roomSquare)){
				return false;
			}
		}
		return true;
	}
	#endregion Resize
	public bool isAddValid(Vector3 cor1, Vector3 cor2) {
		Square testSquare = new Square (cor1, cor2);
		if (testSquare.LLCorner.x >= testSquare.URCorner.x) {
			return false;
		}
		if (testSquare.LLCorner.z >= testSquare.URCorner.z) {
			return false;
		}
		Square roomSquare;
		foreach (ARTFRoom room in roomList) {
			roomSquare = new Square(room.LLCorner, room.URCorner);
			if(testSquare.Intersect(roomSquare)){
				return false;
			}
		}
		return true;
	}

	public bool isStartOrEndRoomValid(Vector3 cor1, Vector3 cor2){
		float minSize = 5f;

		if(Mathf.Abs(cor1.x - cor2.x) < minSize || Mathf.Abs(cor1.z - cor2.z) < minSize){
			Debug.Log("Starting and ending rooms must be at least " + minSize + "x" + minSize + " in size");
			return false;
		}
		return true;
	}

	#endregion Validation

	/*
	 * public ARTFRoom find(Vector3 pos)
	 * 
	 * Gets the room at a given position
	 */
	protected internal ARTFRoom find(Vector3 pos) {
		//for each extant room
		foreach(ARTFRoom rm in roomList) {
			//if the position is
			if(rm.inRoom(pos)) {
				return rm;
			}
		}
		return null;
	}

}