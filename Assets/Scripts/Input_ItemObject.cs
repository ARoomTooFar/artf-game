using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Input_ItemObject : MonoBehaviour {
	public Output_ItemObject itemObjectOutput;

	public LayerMask draggingLayerMask;
	static Camera UICamera;
	GameObject itemObjectBeingDragged;
	bool inMouseCheck = false;
	Vector3 initMousePos;
	ItemClass itemClass = new ItemClass();
	TileMap tileMap;
	float mouseDeadZone = 10f;
	

	void Start () {
		UICamera = GameObject.Find ("UICamera").camera;
		tileMap = GameObject.Find ("TileMap").GetComponent("TileMap") as TileMap;
	}
	

	void Update () {
		if (!Input.GetMouseButtonDown (0)) 
			return; 
		
		Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit; 
		
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {

			//if this script grabs ahold of the TileMap (or presumably
			//any collider that isn't an object meant to be dragged),
			//it will jack shit up. must catch them here.
			if (hit.collider.gameObject.name != "TileMap") {
				itemObjectBeingDragged = hit.collider.gameObject; 
				if (inMouseCheck == false) {
					initMousePos = Input.mousePosition;
					inMouseCheck = true;
				}
				StartCoroutine (DragObject (hit.distance));
				
			}
		}
	}

	IEnumerator DragObject (float distance)
	{ 
		while (Input.GetMouseButton(0)) { 
			Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			
			Vector3 mouseChange = initMousePos - Input.mousePosition;
			
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, draggingLayerMask)) {
				if (hitInfo.collider.gameObject.name == "TileMap") {
					int x = Mathf.RoundToInt (hitInfo.point.x / tileMap.tileSize);
					int z = Mathf.RoundToInt (hitInfo.point.z / tileMap.tileSize);
					//					BoxCollider objectBoxCollider = draggedObject.GetComponent("BoxCollider") as BoxCollider;
					
					//if mouse left deadzone, begin dragging it
					if (Math.Abs (mouseChange.x) > mouseDeadZone 
					    || Math.Abs (mouseChange.y) > mouseDeadZone 
					    || Math.Abs (mouseChange.z) > mouseDeadZone) {
						
						//for now y-pos remains as prefab's default.
//						itemObjectBeingDragged.transform.position = new Vector3 (x * 1.0f, itemObjectBeingDragged.transform.position.y, z * 1.0f);
						Vector3 newp = new Vector3 (x * 1.0f, itemObjectOutput.getPosition().y, z * 1.0f);
						itemObjectOutput.changePosition(newp);
//						Debug.Log (itemObjectOutput.getPosition());

						//						data.modifyItemList (draggedObject.name, draggedObject.transform.position);
//						itemClass.modifyItemList (itemObjectBeingDragged.name, itemObjectBeingDragged.transform.position);
						itemClass.modifyItemList(itemObjectOutput.getName(), itemObjectOutput.getPosition());
					}
				}
			}
			yield return null; 
		}
		
		inMouseCheck = false;
	}
}
