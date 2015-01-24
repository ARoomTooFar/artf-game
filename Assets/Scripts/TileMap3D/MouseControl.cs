using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {

	// reference to tile map
	TileMap tilemap;

	// current tile
	Vector3 currTile;

	// selecting objects
	GameObject selectionCube;
	GameObject currentObj;

	// reference to a list of objects to instantiate
	GameObject menu;

	// select when mouse over
	bool notselected;
	string selectedObject;

	// Initialize variables, setting booleans
	void Awake () {
		tilemap = GetComponent<TileMap> ();
		notselected = true;
		selectionCube = Instantiate (Resources.Load("selectionCube")) as GameObject;
		selectedObject = null;
	}
	
	void Update () {
		RayToScene ();
	}

	void RayToScene(){
		// get world coordinates with respect to mouse position by raycast
		Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		//Debug.DrawRay(Camera.mainCamera.transform.position, Input.mousePosition);
		
		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			// Debug.Log(hitInfo.collider.gameObject.name);
			if(selectedObject != null && Input.GetMouseButtonDown (0)){
				Vector3 obj_pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
				obj_pos.y = tilemap.transform.position.y;
//				Debug.Log( obj_pos.x + " " + obj_pos.y + " " + obj_pos.z);
				placeItems(selectedObject, obj_pos);
			} else{
				switch(hitInfo.collider.gameObject.name){
				case "selectionCube(Clone)":
					snap2grid(hitInfo.point.x, hitInfo.point.z);
					break;
				case "TileMap":
					snap2grid(hitInfo.point.x, hitInfo.point.z);
					break;
				default: 
					Debug.Log(hitInfo.collider.gameObject.name);
					break;
				}
			}
			//Debug.DrawRay(Camera.mainCamera.transform.position, hitInfo.point);
			//Debug.Log (hitInfo.point);
		} else {
			//renderer.material.color = Color.red;
		}
	}

	void snap2grid(float x, float z){
		x = Mathf.RoundToInt( x / tilemap.tileSize );
		z = Mathf.RoundToInt( z / tilemap.tileSize );
		currTile.x = x;
		currTile.z = z;
		if(notselected) selectionCube.transform.position = currTile;
		
		if (Input.GetMouseButtonDown (0)) {
			selectTile ();
		}
		
		
		if (Input.GetMouseButtonDown (1)) {
			deselect();
		}
	}

	void selectTile(){
		notselected = false;
		selectionCube.transform.position = currTile;
		selectionCube.GetComponent<MeshRenderer> ().material.color = Color.blue;
	}

	void deselect(){
		notselected = true;
		selectionCube.GetComponent<MeshRenderer> ().material.color = Color.green;
	}

	void placeItems(string name, Vector3 position){
		position.x = Mathf.RoundToInt( position.x / tilemap.tileSize );
		position.z = Mathf.RoundToInt( position.z / tilemap.tileSize );
		currentObj = Instantiate (Resources.Load(name), position, Quaternion.identity) as GameObject;
	}
}
