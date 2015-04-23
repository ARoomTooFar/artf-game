using UnityEngine;
using System.Collections;

public static class StartEndRoom{
	static TileMapController tilemapcont;

	public static bool startRoomPlaced = false;
	public static bool endRoomPlaced = false;
	public static Vector3 playerStartingLocation;
	public static Vector3 dungeonEndingLocation;

	public static GameObject StartRoomCheckMark;
	public static GameObject EndRoomCheckMark;

	static StartEndRoom(){
		tilemapcont = Camera.main.GetComponent<TileMapController>();
		StartRoomCheckMark = GameObject.Find("StartRoomCheckMark");
		EndRoomCheckMark = GameObject.Find("EndRoomCheckMark");
	}
	
	public static void placeStartRoom(){
		if(!startRoomPlaced){
			//startRoomPlaced = tilemapcont.fillInStartRoom();
			if(startRoomPlaced){
				//playerStartingLocation = tilemapcont.getCenterOfSelectedArea();
				MapData.addObject("Prefabs/PlayerStartingLocation", playerStartingLocation, DIRECTION.North);
				//GameObject.Instantiate(Resources.Load("Prefabs/PlayerStartingLocation"), playerStartingLocation, Quaternion.identity);
				StartRoomCheckMark.SetActive(true);
			}
		}else{
			Debug.Log ("Start room already exists");
		}
	}

	public static void placeEndRoom(){
		Debug.Log("End Room");
		if(!endRoomPlaced){
			//endRoomPlaced = tilemapcont.fillInEndRoom();
			if(endRoomPlaced){
				//dungeonEndingLocation = tilemapcont.getCenterOfSelectedArea();
				MapData.addObject("Prefabs/PlayerEndingLocation", dungeonEndingLocation, DIRECTION.North);
				//GameObject.Instantiate(Resources.Load("Prefabs/EndingRoomLocation"), dungeonEndingLocation, Quaternion.identity);
				EndRoomCheckMark.SetActive(true);
			}
		}else{
			Debug.Log ("End room already exists");
		}
	}
}
