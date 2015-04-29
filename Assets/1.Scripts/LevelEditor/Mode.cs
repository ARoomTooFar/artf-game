using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Controls the mode of the level editor
 * 
 */

public static class Mode
{
	static bool roomMode = false;
	static bool tileMode = true;

	static Text modeText;

	static Mode(){
		modeText = GameObject.Find("ModeText").GetComponent<Text>();
	}

	public static void setRoomMode ()
	{
		roomMode = true;
		tileMode = false;

		foreach (ARTFRoom rm in MapData.TheFarRooms.roomList) {
			rm.setMarkerActive(true);
		}

		modeText.text = "R";
	}

	public static void setTileMode ()
	{
		roomMode = false;
		tileMode = true;

		foreach (ARTFRoom rm in MapData.TheFarRooms.roomList) {
			rm.setMarkerActive(false);
		}

		modeText.text = "T";
	}

	public static bool isTileMode ()
	{
		return tileMode;
	}

	public static bool isRoomMode ()
	{
		return roomMode;
	}

}
