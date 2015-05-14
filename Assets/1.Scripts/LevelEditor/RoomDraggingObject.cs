using UnityEngine;
using System;
using System.Collections;

public class RoomDraggingObject : ClickEvent {
	public LayerMask draggingLayerMask;
	Camera UICamera;
	CameraDraws camDraw;
	CameraRaycast camCast;
	TileMapController tilemapcont;

	void Start() {
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		camCast = UICamera.GetComponent<CameraRaycast>();
		camDraw = UICamera.GetComponent<CameraDraws>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();
	}
		
	public override IEnumerator onClick(Vector3 initPosition) {
		if(!Mode.isRoomMode()) {
			return false;
		}

		Vector3 origin = camCast.mouseGroundPoint.Round();
		camDraw.room = MapData.TheFarRooms.find(origin);

		//for the ghost-duplicate
		Vector3 position = origin;
		tilemapcont.suppressDragSelecting = true;
		while(Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)) {
				Debug.Log("Cancel");
				return false;
			}

			Vector3 mouseChange = initPosition - Input.mousePosition;

			position = camCast.mouseGroundPoint.Round();

			//if mouse left deadzone
			if(Math.Abs(mouseChange.x) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.y) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.z) > Global.mouseDeadZone) {

				//for now y-pos remains as prefab's default.
				camDraw.roomOffset = position-origin;
			}	

			yield return null; 
		}

		camDraw.room = null;
		camDraw.roomOffset = Global.nullVector3;


		tilemapcont.suppressDragSelecting = false;
		tilemapcont.deselect(origin);
		MapData.moveRoom(origin, position);
		tilemapcont.selectTile(position);
	
	}

	public Vector3 getPosition() {
		return this.gameObject.transform.position.Round();
	}
	
	public Quaternion getRotation() {
		return this.gameObject.transform.rotation;
	}
}