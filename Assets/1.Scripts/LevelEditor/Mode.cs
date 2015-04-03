using UnityEngine;
using System.Collections;

/*
 * Controls the mode of the level editor
 * 
 */

public static class Mode
{
	static bool roomMode = false;
	static bool tileMode = true;

	static GameObject Button_TileMode_Background = GameObject.Find("Button_TileMode_Background");
	static GameObject Button_RoomMode_Background = GameObject.Find("Button_RoomMode_Background");

	static Mode(){
		Button_TileMode_Background = GameObject.Find("Button_TileMode_Background");
		Button_RoomMode_Background = GameObject.Find("Button_RoomMode_Background");
	}

	public static void setRoomMode ()
	{
		roomMode = true;
		tileMode = false;

		Button_RoomMode_Background.SetActive(true);
		Button_TileMode_Background.SetActive(false);
	}

	public  static void setTileMode ()
	{
		roomMode = false;
		tileMode = true;

		Button_RoomMode_Background.SetActive(false);
		Button_TileMode_Background.SetActive(true);
	}

	public  static bool isTileMode ()
	{
		return tileMode;
	}

	public  static bool isRoomMode ()
	{
		return roomMode;
	}

}
