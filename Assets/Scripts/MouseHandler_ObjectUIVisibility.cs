using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;


//This class manages clicks to objects, which cause their object UI
//to show up.
public class MouseHandler_ObjectUIVisibility : MonoBehaviour {
	bool rayHit = false;
	Vector3 mouseStartPos = new Vector3(0,0,0);
	public Camera cam;
	public GameObject toggle; //the object to toggle
	public GameObject thing;

	void Start () {
		cam = GameObject.Find("UICamera").camera;
	}

	void Update () {
		RaycastHit hit;

		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit);
			if (hit.collider && (hit.collider.gameObject.GetInstanceID() == thing.GetInstanceID())){
				rayHit = true;
				mouseStartPos = Input.mousePosition;
			}
		}

		//this is to check if a drag has been performed.
		//if a drag has been performed, the game will register it
		//as a click. we have to prevent this by checking if the mouse
		//has moved since the last mouseButtonDown. if it has, it means
		//we're dragging, and so we choose not to toggle the object UI.
		if (Input.GetMouseButtonUp(0) && rayHit == true){ 
			Vector3 offset = Input.mousePosition - mouseStartPos;

			//if the offeset is not zero, then a drag happened
			if (Math.Abs(offset.x) == 0){
				toggle.SetActive (!toggle.activeSelf);
			}
			rayHit = false;
		}
	}
}
