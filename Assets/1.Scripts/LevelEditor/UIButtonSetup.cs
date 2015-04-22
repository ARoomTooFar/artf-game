//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSetup : MonoBehaviour {
	void Start(){

		Button btn;

		btn = GameObject.Find("Button_Room").GetComponent("Button") as Button;
		btn.onClick.AddListener(() => {
			Camera.main.GetComponent<TileMapController>().fillInRoom(); });

		btn = GameObject.Find ("Button_TopDown").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			Camera.main.GetComponent<CameraMovement>().changeToTopDown (); });

		btn = GameObject.Find ("Button_Perspective").GetComponent("Button") as Button;
		btn.onClick.AddListener(() => {
			Camera.main.GetComponent<CameraMovement>().changeToPerspective();});

		btn = GameObject.Find ("Button_Orthographic").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			Camera.main.GetComponent<CameraMovement>().changetoOrthographic ();});

		btn = GameObject.Find ("Button_ZoomOut").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			Camera.main.GetComponent<CameraMovement>().zoomCamIn ();});

		btn = GameObject.Find ("Button_ZoomIn").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			Camera.main.GetComponent<CameraMovement>().zoomCamOut ();});

		//Button_Hand = GameObject.Find ("Button_Hand").GetComponent("Button") as Button;
		//Button_Pointer = GameObject.Find ("Button_Pointer").GetComponent("Button") as Button;

		btn = GameObject.Find ("Button_TileMode").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			Mode.setTileMode();
		});

		btn = GameObject.Find ("Button_RoomMode").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			Mode.setRoomMode();
		});



		btn = GameObject.Find ("Button_StartRoom").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			StartEndRoom.placeStartRoom();
		});
		StartEndRoom.StartRoomCheckMark.SetActive(false);

		btn = GameObject.Find ("Button_EndRoom").GetComponent("Button") as Button;
		btn.onClick.AddListener (() => {
			StartEndRoom.placeEndRoom();
		});
		StartEndRoom.EndRoomCheckMark.SetActive(false);

		//Mode.setTileMode();
	}
}
