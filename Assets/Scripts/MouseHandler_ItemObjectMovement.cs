using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

//This class handles mouse raycasts to objects for the purpose
//of letting the user drag things around.
public class MouseHandler_ItemObjectMovement : MonoBehaviour
{ 
	public TileMap tileMap;
	public GameObject draggedObject;
	public Camera cam;

	public LayerMask draggingLayerMask;

//	int mouseMoved = 0;
//	Vector3 initMousePos;
//	bool inMouseCheck = false;

	DataHandler_Items data;


	void Start(){
		data = GameObject.Find ("ItemObjects").GetComponent("DataHandler_Items") as DataHandler_Items;
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
//				if(inMouseCheck == false){
//					initMousePos = Input.mousePosition;
//					inMouseCheck = true;
//				}

//				Vector3 newMousePos = initMousePos - Input.mousePosition;
//				if(newMousePos.x > 0 || newMousePos.y > 0 || newMousePos.z > 0){
					StartCoroutine (DragObject (hit.distance));
//					inMouseCheck = false;
//				}
			}
		}


	}
	
	IEnumerator DragObject (float distance)
	{ 
		while (Input.GetMouseButton(0)) { 
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, draggingLayerMask)) {
				if(hitInfo.collider.gameObject.name == "TileMap"){
					int x = Mathf.RoundToInt (hitInfo.point.x / tileMap.tileSize);
					int z = Mathf.RoundToInt (hitInfo.point.z / tileMap.tileSize);
//					BoxCollider objectBoxCollider = draggedObject.GetComponent("BoxCollider") as BoxCollider;

					//for now we leave the y position as it is in the original prefab.
					//eventually we should use the object's collder's size/position/center,
					//and the object's scale/position (and maybe also the ground's y value), 
					//in order to calculate what its y value should be.
					draggedObject.transform.position = new Vector3 (x * 1.0f, draggedObject.transform.position.y, z * 1.0f);

					data.modifyItemDictionary(draggedObject.name, draggedObject.transform.position);
				}
			}
			yield return null; 
		}
	}

}