using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Input_ItemObject : MonoBehaviour {
	Output_ItemObject output_itemObject;
	
	public LayerMask draggingLayerMask;
	static Camera UICamera;
	bool inMouseCheck = false;
	Vector3 initMousePos;
	static ItemClass itemClass = new ItemClass();
	TileMap tileMap;
	float mouseDeadZone = 10f;
	
	
	void Start () {
		UICamera = GameObject.Find ("UICamera").camera;
		tileMap = GameObject.Find ("TileMap").GetComponent("TileMap") as TileMap;
		output_itemObject = this.gameObject.GetComponent("Output_ItemObject") as Output_ItemObject;
	}
	
	
	void Update () {
		if (!Input.GetMouseButtonDown (0)) 
			return; 
		
		Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit; 
		
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			
			//check for tilemap so we don't try to drag it
			if (hit.collider.gameObject.name != "TileMap" 
			    && hit.collider.gameObject.GetInstanceID() == this.gameObject.GetInstanceID()) {
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
					//BoxCollider objectBoxCollider = draggedObject.GetComponent("BoxCollider") as BoxCollider;
					
					//if mouse left deadzone, begin dragging it
					if (Math.Abs (mouseChange.x) > mouseDeadZone 
					    || Math.Abs (mouseChange.y) > mouseDeadZone 
					    || Math.Abs (mouseChange.z) > mouseDeadZone) {
						
						//for now y-pos remains as prefab's default.
						Vector3 newp = new Vector3 (x * 1.0f, output_itemObject.getPosition().y, z * 1.0f);
						output_itemObject.changePosition(newp);
						
						//old way
						itemClass.modifyItemList(output_itemObject.getName(), output_itemObject.getPosition(), output_itemObject.getRotation());
						
						//new way
						//						itemClass.changeItemPosition(output_itemObject.getName(), output_itemObject.getPosition());
					}
				}
			}
			yield return null; 
		}
		
		inMouseCheck = false;
	}
}
