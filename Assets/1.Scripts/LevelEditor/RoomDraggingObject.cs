using UnityEngine;
using System;
using System.Collections;

public class RoomDraggingObject : ClickEvent {
	public LayerMask draggingLayerMask;
	Camera UICamera;
	TileMapController tilemapcont;

	void Start() {
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();
	}
		
	public override IEnumerator onClick(Vector3 initPosition) {
		if(!Mode.isRoomMode()) {
			return false;
		}

		Ray ray = UICamera.ScreenPointToRay(initPosition);
		float distance;
		Global.ground.Raycast(ray, out distance);

		Vector3 origin = ray.GetPoint(distance).Round();
		UICamera.GetComponent<CameraDraws>().room = MapData.TheFarRooms.find(origin);

		//for the ghost-duplicate
		Vector3 position = origin;
		tilemapcont.suppressDragSelecting = true;
		while(Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)) {
				Debug.Log("Cancel");
				return false;
			}

			ray = UICamera.ScreenPointToRay(Input.mousePosition);
			Global.ground.Raycast(ray, out distance);
			
			Vector3 mouseChange = initPosition - Input.mousePosition;

			position = ray.GetPoint(distance).Round();
					
			//if mouse left deadzone
			if(Math.Abs(mouseChange.x) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.y) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.z) > Global.mouseDeadZone) {

				//for now y-pos remains as prefab's default.

				UICamera.GetComponent<CameraDraws>().roomOffset = position-origin;
			}	

			yield return null; 
		}

		UICamera.GetComponent<CameraDraws>().room = null;
		UICamera.GetComponent<CameraDraws>().roomOffset = Global.nullVector3;

		tilemapcont.suppressDragSelecting = false;
		tilemapcont.deselect(origin);
		MapData.moveRoom(origin, position);
		tilemapcont.selectTile(position);
	}

	public Vector3 getPosition() {
		return this.gameObject.transform.root.position.Round();
	}
	
	public Quaternion getRotation() {
		return this.gameObject.transform.rotation;
	}
}
