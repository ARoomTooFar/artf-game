using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Input_TileMap : MonoBehaviour
{

	Output_TileMap output_tileMap;
	Camera UICamera;
	TileMap tileMap;
	public HashSet<Vector3> selectedTiles = new HashSet<Vector3> ();
	Vector3 currTile;
	Vector3 shiftOrigin;
	string selectedItem = null;
	GameObject currentObj;

	static ItemClass itemClass = new ItemClass ();
	

	void Start ()
	{
		UICamera = GameObject.Find ("UICamera").camera;
		output_tileMap = this.gameObject.GetComponent ("Output_TileMap") as Output_TileMap;
		tileMap = this.gameObject.GetComponent<TileMap> ();
	}

	void Update ()
	{
		RayToScene ();
	}

	void RayToScene ()
	{
		/* get world coordinates with respect to mouse position by raycast */
		Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		
		/* getting raycast info and logic */
		if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			
			//selectedItem gets set by UIHandler_ItemButtons calling setSelectedItem()
			//in this script. the !Input.GetMouseButton (0) check below will indicate
			//that a drag has ended, and so we can drop the object on the map.
			if (selectedItem != null && !Input.GetMouseButton (0)) {
				int x = Mathf.RoundToInt (hitInfo.point.x / tileMap.tileSize);
				int z = Mathf.RoundToInt (hitInfo.point.z / tileMap.tileSize);
				
				Vector3 obj_pos = new Vector3 (x, 0f, z);
				Vector3 obj_rot = new Vector3 (0f, 90f, 0f);
				output_tileMap.instantiateItemObject (selectedItem, obj_pos, obj_rot);
				clearSelectedItem ();
			} else {
				/* check whether the ray hits an object or the tile map */
				switch (hitInfo.collider.gameObject.name) {
				case "TileMap":
					snapToGrid (hitInfo.point.x, hitInfo.point.z);
					break;
				default: 
					snapToGrid (hitInfo.collider.gameObject.transform.position.x, hitInfo.collider.transform.position.z);
					break;
				}
			}
		}
	}

	/* snap mouse selection to grid */
	void snapToGrid (float xf, float zf)
	{
		int x = Mathf.RoundToInt (xf / tileMap.tileSize);
		int z = Mathf.RoundToInt (zf / tileMap.tileSize);
		
		/* check whether mouse is pressed AND the tile hasn't been selected AND weather we're over a screen UI element */
		if (Input.GetMouseButtonDown (0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			
			/*Control functionality: selects tiles and adds to hashset */
			if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
				/*If the tile already has been selected, deselect it */
				if (!selectedTiles.Add ((new Vector3 (x, 0, z)))) {
					deselect (new Vector3 (x, 0, z));
				}
				/*Otherwise, select it */
				else {
					selectTile (new Vector3 (x, 0, z));
				}
			}
			
			/*Shift functionality: selects all tiles between last selected tile, and shift clicked tile */
			else if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
				/*If no tiles have been selected ever, just select that tile */
				if (shiftOrigin == null)
					selectTile (new Vector3 (x, 0, z));
				
				/*Deselect other tiles, then select all tiles between bounds */
				else {
					deselectAll ();
					Vector3 vec = new Vector3 (x, 0, z);
					Vector3 max = vec.getMaxVals (shiftOrigin);
					Vector3 min = vec.getMinVals (shiftOrigin);
//					Debug.Log (shiftOrigin + " to " + x + ", " + z);
					for (int xx = (int) min.x; xx <= (int) max.x; xx++) {
						for (int zz = (int) min.z; zz <= (int) max.z; zz++) {
							selectedTiles.Add (new Vector3 (xx, 0, zz));
						}
					}
					//fill in selected area with a room
					output_tileMap.fillInRoom(selectedTiles, shiftOrigin.x, shiftOrigin.z, x, z);
				}
				/*Normal click functionality: Deselect all selected, select target */	
			} else {
				deselectAll ();
				selectTile (new Vector3 (x, 0, z));
			}
			
		}
		
	}

	/* Add selected tile index to a list to be access by the camera script for rendering 
	 * and update the last selected tile in case of shift click */
	void selectTile (Vector3 add)
	{
		selectedTiles.Add (add);
		shiftOrigin = add;
	}
	
	/*deselects all tiles */
	void deselectAll ()
	{
		selectedTiles.Clear ();
	}
	
	/*deselects tile passed into function */
	void deselect (Vector3 remove)
	{
		selectedTiles.Remove (remove);
	}
	
	public void setSelectedItem (string s)
	{
		selectedItem = s;
	}
	
	public void clearSelectedItem ()
	{
		selectedItem = null;
	}

	public HashSet<Vector3> getSelectedTiles ()
	{
		return selectedTiles;
	}
}
