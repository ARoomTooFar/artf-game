using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Input_ItemObject : MonoBehaviour
{
	Output_ItemObject output_itemObject;
	public LayerMask draggingLayerMask;
	static Camera UICamera;
	bool inMouseCheck = false;
	Vector3 initMousePos;
	static ItemClass itemClass = new ItemClass ();
	TileMap tileMap;
	float mouseDeadZone = 10f;
	Shader focusedShader;
	Shader nonFocusedShader;
	Vector3 newp;
	
	void Start ()
	{
		UICamera = GameObject.Find ("UICamera").camera;
		tileMap = GameObject.Find ("TileMap").GetComponent ("TileMap") as TileMap;
		output_itemObject = this.gameObject.GetComponent ("Output_ItemObject") as Output_ItemObject;

		focusedShader = Shader.Find ("Transparent/Bumped Diffuse");
		nonFocusedShader = Shader.Find ("Bumped Diffuse");

		this.gameObject.renderer.material.shader = nonFocusedShader;
	}
	
	void Update ()
	{
		if (!Input.GetMouseButtonDown (0) || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == true) 
			return;
		
		Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit; 
		
		if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
			
			//check for tilemap so we don't try to drag it
			if (hit.collider.gameObject.name != "TileMap" 
				&& hit.collider.gameObject.GetInstanceID () == this.gameObject.GetInstanceID ()) {
				if (inMouseCheck == false) {
					initMousePos = Input.mousePosition;
					inMouseCheck = true;
				}
				StartCoroutine (DragObject (hit.distance));
				
			}
		}
	}

	IEnumerator DragObject (float distance)
	{ 
		//for the ghost-duplicate
		GameObject itemObjectCopy = null;
		Output_ItemObject copyOutput = null;

		bool cancellingMove = false;
		bool outOfDeadZone = false;
		bool copyCreated = false;

		while (Input.GetMouseButton(0)) { 

			//if mouse left deadzone, and we haven't made a copy of the object yet
			if (outOfDeadZone && !copyCreated) {
				//create copy of item object
				itemObjectCopy = Instantiate (this.gameObject) as GameObject;
				copyOutput = itemObjectCopy.GetComponent ("Output_ItemObject") as Output_ItemObject;
				copyOutput.changePosition (output_itemObject.getPosition ());
				copyOutput.changeOrientation (output_itemObject.getRotation ());

				//so this code only happens once
				copyCreated = true;
			}

			//if user wants to cancel the drag
			if (Input.GetKeyDown (KeyCode.Escape) || Input.GetMouseButton (1)) {
				Destroy (itemObjectCopy);
				cancellingMove = true;

				//break out of while loop
				break;
			}

			Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			
			Vector3 mouseChange = initMousePos - Input.mousePosition;
			
			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, draggingLayerMask)) {
				if (hitInfo.collider.gameObject.name == "TileMap") {
					int x = Mathf.RoundToInt (hitInfo.point.x / tileMap.tileSize);
					int z = Mathf.RoundToInt (hitInfo.point.z / tileMap.tileSize);

					//if mouse left deadzone
					if (Math.Abs (mouseChange.x) > mouseDeadZone 
						|| Math.Abs (mouseChange.y) > mouseDeadZone 
						|| Math.Abs (mouseChange.z) > mouseDeadZone) {

						outOfDeadZone = true;
						
						//for now y-pos remains as prefab's default.
						newp = new Vector3 (x * 1.0f, output_itemObject.getPosition ().y, z * 1.0f);



						//if copy exists
						if (copyCreated) {
							//update the item object things
							//shader has to be set in this loop, or transparency won't work
							itemObjectCopy.gameObject.renderer.material.shader = focusedShader;
							Color trans = itemObjectCopy.gameObject.renderer.material.color;
							trans.a = 0.5f;
							itemObjectCopy.gameObject.renderer.material.SetColor ("_Color", trans);
							copyOutput.changePosition (new Vector3 (x, output_itemObject.getPosition ().y, z));
							copyOutput.changeOrientation (output_itemObject.getRotation ());
						}

					}
				}

			}
			yield return null; 
		}

		//destroy the copy
		Destroy (itemObjectCopy);

		//if move was cancelled, we don't perform an update on the item object's position
		if (cancellingMove == true) {

		} else {
			output_itemObject.changePosition (newp);
			itemClass.modifyItemList (output_itemObject.getName (), output_itemObject.getPosition (), output_itemObject.getRotation ());
		}

		
		inMouseCheck = false;
	}
}
