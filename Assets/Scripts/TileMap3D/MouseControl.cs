using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseControl : MonoBehaviour {

	/* reference to tile map */
	TileMap tilemap;

	/* current tile */
	Vector3 currTile;

	/* selecting objects */
	GameObject selectionCube;
	GameObject currentObj;

	public List<Vector3> selectedTiles;

	/* reference to a list of objects to instantiate */
	GameObject menu;

	/* select when mouse over */
	string selectedObject;

	/* */
	bool[,] isTileSelected;

	/* Initialize variables, setting booleans */
	void Start () {
		tilemap = GetComponent<TileMap> ();
		isTileSelected = new bool[tilemap.grid_x, tilemap.grid_z];
		Debug.Log (tilemap.grid_x + " " + tilemap.grid_z);
		for (int x = 0; x < tilemap.grid_x; ++x) {
			for (int z = 0; z < tilemap.grid_z; ++z){
				isTileSelected[x, z] = false;
			}
		}
		selectedObject = null;
		selectedTiles = new List<Vector3> ();
	}

	/* Calling raycast function */
	void Update () {
		RayToScene ();
	}

	/* raycasting info and logic happens here */
	void RayToScene(){
		/* get world coordinates with respect to mouse position by raycast */
		Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		/* getting raycast info and logic */
		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {

			/* check if an object is selected and whether mouse is pressed */
			if(selectedObject != null && Input.GetMouseButtonDown (0)){
				Vector3 obj_pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
				obj_pos.y = tilemap.transform.position.y;
				placeItems(selectedObject, obj_pos);
			
			} else{

				/* check whether the ray hits an object or the tile map */

				switch(hitInfo.collider.gameObject.name){
				case "TileMap":
					snap2grid(hitInfo.point.x, hitInfo.point.z);
					break;
				default: 
					Debug.Log(hitInfo.collider.gameObject.name);
					break;
				}
			}
		}
	}

	/* snap mouse selection to grid */
	void snap2grid(float xf, float zf){

//		Debug.Log (xf + " " + zf);

		int x = Mathf.FloorToInt( xf / tilemap.tileSize );
		int z = Mathf.FloorToInt( zf / tilemap.tileSize );
		
		/* check whether mouse is pressed AND the tile hasn't been selected */
		if (Input.GetMouseButtonDown (0) && !isTileSelected[x, z]) {
			selectTile (x, z);
		}

		if (Input.GetMouseButtonDown (1) && isTileSelected[x, z]) {
			deselect(x, z);
		}
	}


	/* Add selected tile index to a list to be access by the camera script for rendering */
	void selectTile(int x, int z){
		isTileSelected [x, z] = true;
		selectedTiles.Add(new Vector3(x, 0, z));
		Debug.Log (selectedTiles.Count);
	}

	void deselect(int x, int z){
		isTileSelected [x, z] = false;
//		selectionCube.GetComponent<MeshRenderer> ().material.color = Color.green;
	}

	void placeItems(string name, Vector3 position){
		position.x = Mathf.RoundToInt( position.x / tilemap.tileSize );
		position.z = Mathf.RoundToInt( position.z / tilemap.tileSize );
		currentObj = Instantiate (Resources.Load(name), position, Quaternion.identity) as GameObject;
	}
}
