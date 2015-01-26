using UnityEngine;
using System.Collections;

public class AMouseControl : MonoBehaviour {

	// reference to tile map. used for getting grid square size and other tilemap things
	ATileMap tilemap;

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
		//get the tilemap component in the object this script is in
		//this script contains informat
		tilemap = GetComponent<ATileMap> ();
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
		
		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			//if we currently have an object selected, and the left-mouse button has been clicked
			if(selectedObject != null && Input.GetMouseButtonDown (0)){
				//get the coordinates where the mouse has clicked in the world
				//this coordinate will be used for an object's position
				Vector3 obj_pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
				//set the object's height to the plane's height (the ground)
				obj_pos.y = tilemap.transform.position.y;
				//place the selected object at the coordinate we've made
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
