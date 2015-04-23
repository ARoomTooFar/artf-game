using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Linq;

public class TileMapController : MonoBehaviour {
	public int grid_x;
	public int grid_z;
	Camera UICamera;
	public HashSet<Vector3> selectedTiles = new HashSet<Vector3>();
	public Vector3 shiftOrigin = Global.nullVector3;
	public bool placeRoomClicked = false;
	public float secondX;
	public float secondZ;
	public bool suppressDragSelecting;
	Vector3 clickOrigin = Global.nullVector3;
	Vector3 lastClick = Global.nullVector3;
	
	void Start() {	
		UICamera = Camera.main;
		grid_x = 100;
		grid_z = 100;
		//buildMesh();
	}

	void Update() {
		//RayToScene();
		if(EventSystem.current.IsPointerOverGameObject()) {
			return;
		}
		Vector3 point = Global.nullVector3;
		if(!Input.GetMouseButton(0)) {
			clickOrigin = Global.nullVector3;
			return;
		}
		//raycast
		Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		float distance;
		Global.ground.Raycast(ray, out distance);
		Physics.Raycast(ray, out hitInfo, distance);
			
		/* check whether the ray hits an object or the tile map */
		if(hitInfo.collider != null) {
			point = hitInfo.collider.transform.position.Round();
			point.y = 0;
		} else {
			point = ray.GetPoint(distance).Round();
		}
		if(clickOrigin == Global.nullVector3) {
			clickOrigin = point;
		}

		if(Input.GetMouseButtonDown(0)) {
			lastClick = point;
			if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				if(!selectedTiles.Add(point)) {
					selectedTiles.Remove(point);
				} else {
					shiftOrigin = point;
				}
				return;
			}

			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				if(shiftOrigin == Global.nullVector3) {
					shiftOrigin = point;
				}
				selectBlock(shiftOrigin, point);
				return;
			}

			selectedTiles.Clear();
			selectedTiles.Add(point);
			shiftOrigin = point;

			return;
		}  
		if(Input.GetMouseButton(0) && clickOrigin != point && !suppressDragSelecting) {
			selectBlock(shiftOrigin, point);
			lastClick = point;
		}
	}

	private void selectBlock(Vector3 vec1, Vector3 vec2){
		selectedTiles.Clear();
		Vector3 max = vec1.getMaxVals(vec2);
		Vector3 min = vec1.getMinVals(vec2);
		for(int i = (int) min.x; i <= (int) max.x; ++i) {
			for(int j = (int) min.z; j <= (int) max.z; ++j) {
				selectedTiles.Add(new Vector3(i, 0, j));
			}
		}
	}

	public void fillInRoom() {
		MapData.addRoom(shiftOrigin, lastClick);
	}

	void RayToScene() {
		/* get world coordinates with respect to mouse position by raycast */
		Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		float distance;
		Global.ground.Raycast(ray, out distance);
		
		/* getting raycast info and logic */
		//&& UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == false
		Physics.Raycast(ray, out hitInfo, distance);
		/* check whether the ray hits an object or the tile map */
		if(hitInfo.collider != null) {
			snapToGrid(hitInfo.collider.gameObject.transform.position.x, hitInfo.collider.transform.position.z);
		} else {
			snapToGrid(ray.GetPoint(distance).x, ray.GetPoint(distance).z);
		}
	}
	
	/* snap mouse selection to grid */
	void snapToGrid(float xf, float zf) {
		int x = Mathf.RoundToInt(xf);
		int z = Mathf.RoundToInt(zf);

		Vector3 stgVector = new Vector3(x, 0, z);
		if(Input.GetMouseButton(0)) {
			if(clickOrigin == Global.nullVector3) {
				clickOrigin = stgVector;
			}
		} else {
			clickOrigin = Global.nullVector3;
		}

		//for selecting single tiles, and for shift-clicking to select a group of tiles
		//
		//check whether mouse is pressed AND the tile hasn't been selected AND weather we're over a screen UI element
		if(Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			//Control functionality: selects tiles and adds to hashset
			if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				//If the tile already has been selected, deselect it
				if(selectedTiles.Contains(stgVector)) {
					deselect(stgVector);
				}
				//Otherwise, select it
				else {
					selectTile(stgVector);
				}
				return;
			}
			//Shift functionality: selects all tiles between last selected tile, and shift clicked tile
			else if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				//If no tiles have been selected ever, just select that tile
				if(shiftOrigin == Global.nullVector3) {
					selectTile(stgVector);
				}
				//Deselect other tiles, then select all tiles between bounds
				else {
					deselectAll();
					Vector3 vec = new Vector3(x, 0, z);
					Vector3 max = vec.getMaxVals(shiftOrigin);
					Vector3 min = vec.getMinVals(shiftOrigin);
					// Debug.Log (shiftOrigin + " to " + x + ", " + z);
					for(int xx = (int) min.x; xx <= (int) max.x; xx++) {
						for(int zz = (int) min.z; zz <= (int) max.z; zz++) {
							selectedTiles.Add(new Vector3(xx, 0, zz));
						}
					}
					
				}
				//Normal click functionality: Deselect all selected, select target	
			} else {
				deselectAll();
				selectTile(stgVector);
			}
			secondX = Mathf.RoundToInt(xf);
			secondZ = Mathf.RoundToInt(zf);
		}

		//if user is holding down left mouse button, and dragging,
		//we make a box of selected tile and have it resize as
		//the mouse moves around
		if(clickOrigin != stgVector
			&& suppressDragSelecting == false
			&& Input.GetMouseButton(0)
			&& UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			//If no tiles have been selected ever, just select that tile
			if(shiftOrigin == Global.nullVector3) {
				selectTile(stgVector);
			}
			
			//Deselect other tiles, then select all tiles between bounds 
			else {
				deselectAll();
				Vector3 vec = new Vector3(x, 0, z);
				Vector3 max = vec.getMaxVals(shiftOrigin);
				Vector3 min = vec.getMinVals(shiftOrigin);
				//Debug.Log (shiftOrigin + " to " + x + ", " + z);
				for(int xx = (int) min.x; xx <= (int) max.x; xx++) {
					for(int zz = (int) min.z; zz <= (int) max.z; zz++) {
						selectedTiles.Add(new Vector3(xx, 0, zz));
					}
				}
				secondX = Mathf.RoundToInt(xf);
				secondZ = Mathf.RoundToInt(zf);
			}
		}

		if(placeRoomClicked) {
			fillInRoom();
			placeRoomClicked = false;
		}
	}
	
	/* Add selected tile index to a list to be access by the camera script for rendering 
	 * and update the last selected tile in case of shift click */
	public void selectTile(Vector3 add) {
		selectedTiles.Add(add);
		shiftOrigin = add;
	}
	
	/*deselects all tiles */
	void deselectAll() {
		selectedTiles.Clear();
	}
	
	/*deselects tile passed into function */
	public void deselect(Vector3 remove) {
		selectedTiles.Remove(remove);
	}

	public HashSet<Vector3> getSelectedTiles() {
		return selectedTiles;
	}
}
