using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TESTSCRIPT : MonoBehaviour {

	// Use this for initialization
	void Start() {
		MapData.addRoom(new Vector3(0, 0, 0), new Vector3(10, 0, 10));
		List<Vector3> path = Pathfinder.getSingleRoomPath(new Vector3(3, 0, 3), new Vector3(7, 0, 9));
		foreach(Vector3 vec in path){
			print(vec.toCSV());
		}
	}

/*
		MapData.addRoom(new Vector3(0, 0, 0), new Vector3(2, 0, 2));
		MapData.addRoom(new Vector3(0, 0, 3), new Vector3(2, 0, 5));
		MapData.addRoom(new Vector3(3, 0, 0), new Vector3(5, 0, 2));

		ARTFRoom rm1 = MapData.TheFarRooms.find(new Vector3(1, 0, 1));
		ARTFRoom rm2 = MapData.TheFarRooms.find(new Vector3(1, 0, 4));
		ARTFRoom rm3 = MapData.TheFarRooms.find(new Vector3(4, 0, 1));

		rm1.LinkedRooms.Add(rm2);
		rm1.LinkedRooms.Add(rm3);

		rm2.LinkedRooms.Add(rm1);

		rm3.LinkedRooms.Add(rm1);

		List<ARTFRoom> path = Pathfinder.getRoomPath(rm2, rm3);
		print(path[0].LLCorner);
		print(path[1].LLCorner);
*/

/*
		MapData.addRoom(new Vector3(0, 0, 0), new Vector3(1, 0, 1));

		string save = MapData.SaveString;
		print(save);

		MapDataParser.ParseSaveString(save);

		string newSave = MapData.SaveString;
		print(newSave);
*/
}
