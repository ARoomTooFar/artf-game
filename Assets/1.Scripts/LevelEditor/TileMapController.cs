using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TileMapController : MonoBehaviour {
	Camera UICamera;
	CameraRaycast camCast;
	public HashSet<Vector3> selectedTiles = new HashSet<Vector3>();
	public Vector3 shiftOrigin = Global.nullVector3;
	public bool suppressDragSelecting;
	Vector3 clickOrigin = Global.nullVector3;
	public Vector3 lastClick = Global.nullVector3;
	
	void Start() {	
		UICamera = Camera.main;
		camCast = UICamera.GetComponent<CameraRaycast>();
	}

	void Update() {
		if(EventSystem.current.IsPointerOverGameObject()) {
			return;
		}
		Vector3 point = Global.nullVector3;
		if(!Input.GetMouseButton(0)) {
			clickOrigin = Global.nullVector3;
			return;
		}
		//raycast
		//Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
		//RaycastHit hitInfo;
		//float distance;
		//Global.ground.Raycast(ray, out distance);
		//Physics.Raycast(ray, out hitInfo, distance);
			
		/* check whether the ray hits an object or the tile map */
		/*if(hitInfo.collider != null) {
			point = hitInfo.collider.transform.position.Round();
			point.y = 0;
		} else {
			point = ray.GetPoint(distance).Round();
		}*/


		if(camCast.hitDistance < camCast.mouseDistance
		   && camCast.hit.transform != null){
			point = camCast.hit.transform.position.Round();
			point.y = 0;
		} else {
			point = camCast.mouseGroundPoint.Round();
		}
		 
		if(clickOrigin == Global.nullVector3) {
			clickOrigin = point;
		}

		if(Input.GetMouseButtonDown(0)) {
			lastClick = point;
			/*
			if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
				if(!selectedTiles.Add(point)) {
					selectedTiles.Remove(point);
				} else {
					shiftOrigin = point;
				}
				return;
			}*/

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
		if(MapData.addRoom(shiftOrigin, lastClick)) {
			Money.buy(MapData.TheFarRooms.find(shiftOrigin).Cost);
		}
	}

	/* Add selected tile index to a list to be access by the camera script for rendering 
	 * and update the last selected tile in case of shift click */
	public void selectTile(Vector3 add) {
		selectedTiles.Add(add);
		shiftOrigin = add;
	}

	/*deselects tile passed into function */
	public void deselect(Vector3 remove) {
		selectedTiles.Remove(remove);
	}
}
