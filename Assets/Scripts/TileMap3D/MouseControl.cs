using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MouseControl : MonoBehaviour {

	//holds UI camera
	public Camera cam;

	/* reference to tile map */
	TileMap tilemap;

	/* current tile */
	Vector3 currTile;

	/* selecting objects */
	GameObject selectionCube;
	GameObject currentObj;
	

	/*The last selected tile, stored for shift click */
	Vector3 shiftOrigin;

	/*HashSet that stores all selected tiles */
	public HashSet<Vector3> selectedTiles;

	/* select when clicked */
	public string selectedObject;

	/* Initialize variables, setting booleans */
	void Start () {
		tilemap = GetComponent<TileMap> ();
		selectedObject = null;
		selectedTiles = new HashSet<Vector3> ();
	}

	/* Calling raycast function */
	void Update () {
		RayToScene ();
	}

	/* raycasting info and logic happens here */
	void RayToScene(){
		/* get world coordinates with respect to mouse position by raycast */
//		Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		/* getting raycast info and logic */
		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {

			/* check if an object is selected and whether mouse is pressed */
			if(selectedObject != null && Input.GetMouseButtonDown (0)){
//				Vector3 obj_pos = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
				Vector3 obj_pos = cam.ScreenToWorldPoint(Input.mousePosition);
				obj_pos.y = tilemap.transform.position.y;
				placeItems(selectedObject, obj_pos);
			
			} else{

				/* check whether the ray hits an object or the tile map */

				switch(hitInfo.collider.gameObject.name){
				case "TileMap":
					snap2grid(hitInfo.point.x, hitInfo.point.z);
//					Debug.Log("Tile Map");
					break;
				default: 
					snap2grid(hitInfo.collider.gameObject.transform.position.x, hitInfo.collider.transform.position.z);
//					Debug.Log(hitInfo.collider.gameObject.name);
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
		if (Input.GetMouseButtonDown (0)) {

			/*Control functionality: selects tiles and adds to hashset */
			if(Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
			{
				/*If the tile already has been selected, deselect it */
				if( !selectedTiles.Add( (new Vector3(x, 0, z) ) ) ) {
					deselect(new Vector3(x, 0, z));
				}
				/*Otherwise, select it */
				else {
					selectTile (new Vector3(x, 0, z));
				}
			}

			/*Shift functionality: selects all tiles between last selected tile, and shift clicked tile */
			else if(Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			{
				/*If no tiles have been selected ever, just select that tile */
				if(shiftOrigin == null) selectTile(new Vector3(x, 0, z));

				/*Deselect other tiles, then select all tiles between bounds */
				else{
					deselectAll();
					Vector3 vec = new Vector3(x, 0, z);
					Vector3 max = vec.getMaxVals(shiftOrigin);
					Vector3 min = vec.getMinVals(shiftOrigin);
					for(int xx = (int) min.x; xx <= (int) max.x ; xx++){
						for(int zz = (int) min.z; zz <= (int) max.z; zz++){
							selectedTiles.Add(new Vector3(xx, 0, zz));
						}
					}
				}
			/*Normal click functionality: Deselect all selected, select target */	
			} else {
				deselectAll();
				selectTile(new Vector3(x, 0, z));
			}
		}
	}


	/* Add selected tile index to a list to be access by the camera script for rendering 
	 * and update the last selected tile in case of shift click */
	void selectTile(Vector3 add){
		selectedTiles.Add(add);
		shiftOrigin = add;
	}

	/*deselects all tiles */
	void deselectAll(){
		selectedTiles.Clear ();
	}

	/*deselects tile passed into function */
	void deselect(Vector3 remove){
		selectedTiles.Remove (remove);
	}

	void placeItems(string name, Vector3 position){
		position.x = Mathf.RoundToInt( position.x / tilemap.tileSize );
		position.z = Mathf.RoundToInt( position.z / tilemap.tileSize );
		currentObj = Instantiate (Resources.Load(name), position, Quaternion.identity) as GameObject;
	}
}
