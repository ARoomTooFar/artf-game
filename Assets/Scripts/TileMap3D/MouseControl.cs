using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {

	TileMap tilemap;

	// current tile
	Vector3 currTile;

	// select when mouse over
	public Transform selectionCube;

	bool notselected;

	void Start () {
		tilemap = GetComponent<TileMap> ();
		notselected = true;
	}
	
	void Update () {

		// get world coordinates with respect to mouse position by raycast
		Ray ray = Camera.mainCamera.ScreenPointToRay (Input.mousePosition);
		//Debug.Log (Input.mousePosition);
		RaycastHit hitInfo;
		//Debug.DrawRay(Camera.mainCamera.transform.position, Input.mousePosition);

		//float halftile = tilemap.tileSize / 2f;

		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {

			float x = Mathf.RoundToInt( hitInfo.point.x / tilemap.tileSize );
			float z = Mathf.RoundToInt( hitInfo.point.z / tilemap.tileSize );

			currTile.x = x;
			currTile.z = z;

			if(notselected) selectionCube.transform.position = currTile;
			//Debug.DrawRay(Camera.mainCamera.transform.position, hitInfo.point);
			//Debug.Log (hitInfo.point);
		} else {
			//renderer.material.color = Color.red;

		}


		if (Input.GetMouseButtonDown (0)) {
			selectTile ();
			notselected = false;
		}
	}

	void selectTile(){
		selectionCube.transform.position = currTile;
		selectionCube.GetComponent<MeshRenderer> ().material.color = Color.blue;
	}
}
