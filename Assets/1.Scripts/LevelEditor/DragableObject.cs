using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class DragableObject : ClickEvent {

	Camera UICamera;
	CameraRaycast camCast;
	TileMapController tilemapcont;

	void Start() {
		UICamera = Camera.main;
		camCast = UICamera.GetComponent<CameraRaycast>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();
	}
		
	public override IEnumerator onClick(Vector3 initPosition) {
		if(!Mode.isTileMode()) {
			return false;
		}

		//for the ghost-duplicate
		GameObject itemObjectCopy = null;
		Vector3 position = this.gameObject.transform.position;
		tilemapcont.suppressDragSelecting = true;
		Vector3 mouseChange; 
		while(Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)) {
				Debug.Log("canceled");
				Destroy(itemObjectCopy);
				return false;
			}
			
			mouseChange = initPosition - Input.mousePosition;

			//position = ray.GetPoint(distance).Round();
			position = camCast.mouseGroundPoint.Round();

			//if mouse left deadzone
			if(Math.Abs(mouseChange.x) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.y) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.z) > Global.mouseDeadZone) {
						
				if(itemObjectCopy == null) {
					//create copy of item object
					itemObjectCopy = Instantiate(this.gameObject, getPosition(), getRotation()) as GameObject;

					Color trans;
					//update the item object things
					//shader has to be set in this loop, or transparency won't work
					//itemObjectCopy.gameObject.GetComponentInChildren<Renderer>().material.shader = focusedShader;
					if(itemObjectCopy.GetComponentsInChildren<Renderer>() != null){
						foreach(Renderer rend in itemObjectCopy.GetComponentsInChildren<Renderer>()) {
							foreach(Material mat in rend.materials) {
								if(mat.HasProperty("_Color")){
									trans = mat.color;
									trans.a *= .5f;
									mat.color = trans;
								}
							}
						}
					}
				} else {
					itemObjectCopy.transform.position = position;
					itemObjectCopy.transform.rotation = getRotation();
				}

				//for now y-pos remains as prefab's default.

			}
			yield return null; 
		}
		
		tilemapcont.suppressDragSelecting = false;

		if(itemObjectCopy == null) {
			return false;
		}

		//destroy the copy
		Destroy(itemObjectCopy);
		tilemapcont.deselect(getPosition());
		MapData.dragObject(this.gameObject, getPosition(), position - getPosition());
		tilemapcont.selectTile(position);
	}

	public Vector3 getPosition() {
		return this.gameObject.transform.position;
	}
	
	public Quaternion getRotation() {
		return this.gameObject.transform.rotation;
	}
}
