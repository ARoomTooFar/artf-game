using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class handles mouse clicks on the tile map.
//It maintains a HashSet of all tiles currently selected.
public class MouseHandler_TileSelection : MonoBehaviour {

	//holds UI camera
	public Camera cam;

	/* reference to tile map */
	TileMap tileMap;

	/* current tile */
	Vector3 currTile;

	/*The last selected tile, stored for shift click */
	Vector3 shiftOrigin;

	/*HashSet that stores all selected tiles */
	public HashSet<Vector3> selectedTiles;





	//This is the temporary home for object placement

	/* select when clicked */
	private string selectedObject;

	/* selecting objects */
	GameObject currentObj;

	public void setSelectedObject(string s){
		selectedObject = s;
	}
	
	public void clearSelectedObject(){
		selectedObject = null;
	}
	
	//place a prefab from resources folder
	void placeItems(string name, Vector3 position){
		position.x = Mathf.RoundToInt( position.x / tileMap.tileSize );
		position.z = Mathf.RoundToInt( position.z / tileMap.tileSize );
		currentObj = Instantiate (Resources.Load(name), position, Quaternion.identity) as GameObject;
		clearSelectedObject();
	}






	//Actual mouse handling starts here

	/* Initialize variables, setting booleans */
	void Start () {
		tileMap = GetComponent<TileMap> ();
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
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		/* getting raycast info and logic */
		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {

			/* check if an object is selected and whether mouse is pressed */
			if(selectedObject != null && Input.GetMouseButtonDown (0)){
				int x = Mathf.RoundToInt( hitInfo.point.x / tileMap.tileSize );
				int z = Mathf.RoundToInt( hitInfo.point.z / tileMap.tileSize );

				Vector3 obj_pos = new Vector3(x, 0f, z);
				placeItems(selectedObject, obj_pos);
			} else{
				/* check whether the ray hits an object or the tile map */
				switch(hitInfo.collider.gameObject.name){
				case "TileMap":
					snap2grid(hitInfo.point.x, hitInfo.point.z);
					break;
				default: 
					snap2grid(hitInfo.collider.gameObject.transform.position.x, hitInfo.collider.transform.position.z);
					break;
				}
			}


		}
	}

	/* snap mouse selection to grid */
	void snap2grid(float xf, float zf){
		int x = Mathf.RoundToInt( xf / tileMap.tileSize );
		int z = Mathf.RoundToInt( zf / tileMap.tileSize );
		
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

}
