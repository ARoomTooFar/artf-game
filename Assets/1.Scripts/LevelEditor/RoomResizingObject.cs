using UnityEngine;
using System;
using System.Collections;

public class RoomResizingObject : ClickEvent {
	
	public LayerMask draggingLayerMask = LayerMask.GetMask("Walls");
	Camera UICamera;
	TileMapController tilemapcont;
	Shader focusedShader;
	
	void Start() {
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		tilemapcont = GameObject.Find("TileMap").GetComponent("TileMapController") as TileMapController;
		
		focusedShader = Shader.Find("Transparent/Bumped Diffuse");
	}
	
	public override IEnumerator onClick(Vector3 initPosition) {
		if(!Mode.isRoomMode()) {
			return false;
		}
		
		//for the ghost-duplicate
		GameObject itemObjectCopy = null;
		Vector3 newp = this.gameObject.transform.position;
		UICamera.GetComponent<CameraDraws>().room = MapData.TheFarRooms.find(newp);
		UICamera.GetComponent<CameraDraws>().roomResizeOrigin = newp;
		tilemapcont.suppressDragSelecting = true;
		while(Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)) {
				Debug.Log("Cancel");
				Destroy(itemObjectCopy);
				return false;
			}
			
			Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
			float distance;
			Global.ground.Raycast(ray, out distance);
			
			Vector3 mouseChange = initPosition - Input.mousePosition;

			int x = Mathf.RoundToInt(ray.GetPoint(distance).x);
			int z = Mathf.RoundToInt(ray.GetPoint(distance).z);
				
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
					itemObjectCopy.transform.position = new Vector3(x, getPosition().y, z);
					itemObjectCopy.transform.rotation = getRotation();
				}
					
				//for now y-pos remains as prefab's default.
				newp = new Vector3(x * 1.0f, getPosition().y, z * 1.0f);
				UICamera.GetComponent<CameraDraws>().roomResize = newp;
			}	

			yield return null; 
		}
		
		tilemapcont.suppressDragSelecting = false;
		UICamera.GetComponent<CameraDraws>().room = null;
		UICamera.GetComponent<CameraDraws>().roomResize = Global.nullVector3;
		UICamera.GetComponent<CameraDraws>().roomResizeOrigin = Global.nullVector3;
		//destroy the copy
		Destroy(itemObjectCopy);
		tilemapcont.deselect(getPosition());
		MapData.dragObject(this.gameObject, getPosition(), newp - getPosition());
		tilemapcont.selectTile(newp);
	}
	
	public Vector3 getPosition() {
		return this.gameObject.transform.root.position;
	}
	
	public Quaternion getRotation() {
		return this.gameObject.transform.rotation;
	}
}
