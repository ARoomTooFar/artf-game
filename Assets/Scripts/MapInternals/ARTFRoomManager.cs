using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARTFRoomManager {

	private List<ARTFRoom> roomList = new List<ARTFRoom>();

	public ARTFRoomManager() {
	}

	public bool addRoom(Vector3 pos1, Vector3 pos2){
		ARTFRoom nRoom = new ARTFRoom(pos1, pos2);
		if(doAnyRoomsIntersect(nRoom)){
			return false;
		}
		roomList.Add(nRoom);
		nRoom.linkTerrain();
		return true;
	}

	public bool moveRoom(Vector3 position, Vector3 offset){
		ARTFRoom rm = getRoom(position);
		if(rm == null) {
			return false;
		}
		return rm.Move(offset);
	}

	public bool resizeRoom(Vector3 corner, Vector3 nCorner){
		ARTFRoom rm = getRoom(corner);
		if(rm == null) {
			return false;
		}
		return rm.Resize(corner, nCorner);
	}

	public bool doAnyRoomsIntersect(ARTFRoom room){
		foreach(ARTFRoom rm in roomList) {
			if(rm.Equals(room)) continue;

			if(doRoomsIntersect(rm, room)){
				return true;
			}
		}
		return false;
	}

	public bool doAnyRoomsIntersect(ARTFRoom room, Vector3 offset){
		ARTFRoom tempRoom = new ARTFRoom(room.LLCorner.Add(offset), room.URCorner.Add(offset));
		return doAnyRoomsIntersect(tempRoom);
	}

	public bool doRoomsIntersect(ARTFRoom rm1, ARTFRoom rm2){
		foreach(Vector3 corn in rm1.Corners) {
			if(rm2.inRoom(corn)){
				return true;
			}
		}
		foreach(Vector3 corn in rm2.Corners) {
			if(rm1.inRoom(corn)){
				return true;
			}
		}
		return false;
	}

	private ARTFRoom getRoom(Vector3 position){
		foreach(ARTFRoom rm in roomList) {
			if(rm.inRoom(position)){
				return rm;
			}
		}
		return null;
	}

}