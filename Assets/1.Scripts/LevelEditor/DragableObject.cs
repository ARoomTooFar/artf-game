using UnityEngine;
using System;
using System.Collections;

public class DragableObject : ClickEvent
{

	public LayerMask draggingLayerMask = LayerMask.GetMask ("Walls");
	Camera UICamera;
	TileMapController tilemapcont;
	float mouseDeadZone = 10f;
	Shader focusedShader;
	Shader nonFocusedShader;

	void Start ()
	{
		UICamera = GameObject.Find ("UICamera").GetComponent<Camera> ();
		tilemapcont = GameObject.Find ("TileMap").GetComponent ("TileMapController") as TileMapController;
		
		focusedShader = Shader.Find ("Transparent/Bumped Diffuse");
		nonFocusedShader = Shader.Find ("Bumped Diffuse");
		
		this.gameObject.GetComponentInChildren<Renderer> ().material.shader = nonFocusedShader;
	}
		
	public override IEnumerator onClick (Vector3 initPosition)
	{
		if(!Mode.isTileMode()) {
			return false;
		}

		//for the ghost-duplicate
		GameObject itemObjectCopy = null;
		Vector3 newp = this.gameObject.transform.position;
		tilemapcont.suppressDragSelecting = true;
		while (Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if (Input.GetKeyDown (KeyCode.Escape) || Input.GetMouseButton (1)) {
				Debug.Log ("Cancel");
				Destroy (itemObjectCopy);
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
						
					if (itemObjectCopy == null) {
						//create copy of item object
						itemObjectCopy = Instantiate (this.gameObject, getPosition (), getRotation ()) as GameObject;

						//update the item object things
						//shader has to be set in this loop, or transparency won't work
						//itemObjectCopy.gameObject.GetComponentInChildren<Renderer>().material.shader = focusedShader;
						foreach (Renderer rend in itemObjectCopy.GetComponentsInChildren<Renderer>()) {
							rend.material.shader = focusedShader;
							Color trans = rend.material.color;
							trans.a = .5f;
							rend.material.color = trans;
						}
					} else {
						itemObjectCopy.transform.position = new Vector3 (x, getPosition ().y, z);
						itemObjectCopy.transform.rotation = getRotation ();
					}

					//for now y-pos remains as prefab's default.
					newp = new Vector3 (x * 1.0f, getPosition ().y, z * 1.0f);
				}	
			}
			yield return null; 
		}
		
		tilemapcont.suppressDragSelecting = false;
		
		//destroy the copy
		Destroy (itemObjectCopy);
		tilemapcont.deselect (getPosition());
		MapData.dragObject (this.gameObject, getPosition(), newp - getPosition());
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
