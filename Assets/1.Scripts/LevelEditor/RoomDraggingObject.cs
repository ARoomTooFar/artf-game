using UnityEngine;
using System;
using System.Collections;

public class RoomDraggingObject : ClickEvent
{
	public LayerMask draggingLayerMask;
	Camera UICamera;
	TileMapController tilemapcont;
	float mouseDeadZone = 10f;
	Shader nonFocusedShader;

	void Start ()
	{
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = GameObject.Find ("UICamera").GetComponent<Camera> ();
		tilemapcont = GameObject.Find ("TileMap").GetComponent ("TileMapController") as TileMapController;
		nonFocusedShader = Shader.Find ("Bumped Diffuse");
		
		this.gameObject.GetComponentInChildren<Renderer> ().material.shader = nonFocusedShader;
	}
		
	public override IEnumerator onClick (Vector3 initPosition)
	{
		if(!Mode.isRoomMode()) {
			return false;
		}

		//for the ghost-duplicate
		Vector3 newp = this.gameObject.transform.position;
		tilemapcont.suppressDragSelecting = true;
		while (Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if (Input.GetKeyDown (KeyCode.Escape) || Input.GetMouseButton (1)) {
				Debug.Log ("Cancel");
				return false;
			}
			
			Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			
			Vector3 mouseChange = initPosition - Input.mousePosition;

			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, draggingLayerMask)) {
				int x = Mathf.RoundToInt (hitInfo.point.x / tilemapcont.tileSize);
				int z = Mathf.RoundToInt (hitInfo.point.z / tilemapcont.tileSize);
					
				//if mouse left deadzone
				if (Math.Abs (mouseChange.x) > mouseDeadZone 
					|| Math.Abs (mouseChange.y) > mouseDeadZone 
					|| Math.Abs (mouseChange.z) > mouseDeadZone) {

					//for now y-pos remains as prefab's default.
					newp = new Vector3 (x * 1.0f, getPosition ().y, z * 1.0f);
				}	
			}
			yield return null; 
		}
		
		tilemapcont.suppressDragSelecting = false;
		tilemapcont.deselect (getPosition());
		MapData.moveRoom(getPosition(), newp);
		tilemapcont.selectTile (newp);
	}

	public Vector3 getPosition ()
	{
		return this.gameObject.transform.root.position;
	}
	
	public Quaternion getRotation ()
	{
		return this.gameObject.transform.rotation;
	}
}
