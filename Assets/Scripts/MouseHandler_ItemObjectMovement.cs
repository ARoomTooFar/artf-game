using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

//This class handles mouse raycasts to objects for the purpose
//of letting the user drag things around.
public class MouseHandler_ItemObjectMovement : MonoBehaviour
{ 
	public TileMap tileMap;
	public GameObject draggedObject;
	public Camera cam;
	public LayerMask draggingLayerMask;
	int mouseMoved = 0;
	float mouseDeadZone = 10f;
	Vector3 initMousePos;
	bool inMouseCheck = false;
	DataHandler_Items data;

	ItemClass itemClass = new ItemClass();

	void Start ()
	{
		data = GameObject.Find ("ItemObjects").GetComponent ("DataHandler_Items") as DataHandler_Items;
	}
	
	void Update ()
	{ 
		if (!Input.GetMouseButtonDown (0)) 
			return; 

		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit; 
			
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {

			//WORK IN PROGRESS
			//if this script grabs ahold of the TileMap (or presumably
			//any collider that isn't an object meant to be dragged),
			//it will jack shit up. must catch them here.
			if (hit.collider.gameObject.name != "TileMap") {

				draggedObject = hit.collider.gameObject; 
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
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
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
						draggedObject.transform.position = new Vector3 (x * 1.0f, draggedObject.transform.position.y, z * 1.0f);

//						data.modifyItemList (draggedObject.name, draggedObject.transform.position);
						itemClass.modifyItemList (draggedObject.name, draggedObject.transform.position);
					}
				}
			}
			yield return null; 
		}

		inMouseCheck = false;
	}

}