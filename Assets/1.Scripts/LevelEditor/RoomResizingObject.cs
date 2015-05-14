using UnityEngine;
using System;
using System.Collections;

public class RoomResizingObject : ClickEvent {
	
	public LayerMask draggingLayerMask = LayerMask.GetMask("Walls");
	Camera UICamera;
	CameraRaycast camCast;
	CameraDraws camDraw;
	TileMapController tilemapcont;
	Shader focusedShader;
	
	void Start() {
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		camCast = UICamera.GetComponent<CameraRaycast>();
		camDraw = UICamera.GetComponent<CameraDraws>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();	
		focusedShader = Shader.Find("Transparent/Bumped Diffuse");
	}
	
	public override IEnumerator onClick(Vector3 initPosition) {
		if(!Mode.isRoomMode()) {
			return false;
		}
		
		//for the ghost-duplicate
		GameObject itemObjectCopy = null;
		Vector3 position = this.gameObject.transform.position;
		camDraw.room = MapData.TheFarRooms.find(position);
		camDraw.roomResizeOrigin = position;
		tilemapcont.suppressDragSelecting = true;
		while(Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)) {
				Destroy(itemObjectCopy);
				return false;
			}
			
			Vector3 mouseChange = initPosition - Input.mousePosition;

			position = camCast.mouseGroundPoint.Round();

			//if mouse left deadzone
			if(Math.Abs(mouseChange.x) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.y) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.z) > Global.mouseDeadZone) {
					
				if(itemObjectCopy == null) {
					//create copy of item object
					itemObjectCopy = Instantiate(this.gameObject, getPosition(), getRotation()) as GameObject;
						
					//update the item object things
					//shader has to be set in this loop, or transparency won't work
					//itemObjectCopy.gameObject.GetComponentInChildren<Renderer>().material.shader = focusedShader;
					foreach(Renderer rend in itemObjectCopy.GetComponentsInChildren<Renderer>()) {
						rend.material.shader = focusedShader;
						Color trans = rend.material.color;
						trans.a = .5f;
						rend.material.color = trans;
					}
				} else {
					itemObjectCopy.transform.position = position;
					itemObjectCopy.transform.rotation = getRotation();
				}

				camDraw.roomResize = position;
			}	

			yield return null; 
		}
		
		tilemapcont.suppressDragSelecting = false;
		camDraw.room = null;
		camDraw.roomResize = Global.nullVector3;
		camDraw.roomResizeOrigin = Global.nullVector3;
		//destroy the copy
		Destroy(itemObjectCopy);
		tilemapcont.deselect(getPosition());
		MapData.resizeRoom(this.gameObject, getPosition(), position - getPosition());
		tilemapcont.selectTile(position);
	}
	
	public Vector3 getPosition() {
		return this.transform.position;
	}
	
	public Quaternion getRotation() {
		return this.transform.rotation;
	}
}
