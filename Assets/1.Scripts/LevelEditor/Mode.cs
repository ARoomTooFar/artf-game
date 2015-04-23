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

//	static GameObject Button_TileMode_Background;
//	static GameObject Button_RoomMode_Background;
	static Text modeText;

	static GameObject RoomPanel;

	static Mode(){
//		Button_TileMode_Background = GameObject.Find("Button_TileMode_Background");
//		Button_RoomMode_Background = GameObject.Find("Button_RoomMode_Background");
		RoomPanel = GameObject.Find("RoomPanel");
		modeText = GameObject.Find("ModeText").GetComponent<Text>();
	}

	public static void setRoomMode ()
	{
		roomMode = true;
		tileMode = false;

//		Button_RoomMode_Background.SetActive(true);
//		Button_TileMode_Background.SetActive(false);

		//RoomPanel.SetActive(true);

		foreach (ARTFRoom rm in MapData.TheFarRooms.roomList) {
			rm.setMarkerActive(true);
		}

		modeText.text = "M";
	}

	public static void setTileMode ()
	{
		roomMode = false;
		tileMode = true;

//		Button_RoomMode_Background.SetActive(false);
//		Button_TileMode_Background.SetActive(true);

		//RoomPanel.SetActive(false);

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
