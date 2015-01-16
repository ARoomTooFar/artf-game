using UnityEngine;
using System.Collections;

//This class controls the level editor camera, allowing it
//to switch between ortho/top down perspectives, as well
//as zoom in/out, and move along the x and z axis using
//the arrow keys. Also movement along x and y axis
//via dragging.
public class CameraAdjuster : MonoBehaviour {
	//Base location for camera, limits of camera,
	//boolean control for camera angle, and speed of
	//camera movement, drag speed, and mouse location
	//initialization maxX and maxY tbd based on needs 
	//of level editor, left blank here
	public float baseY, baseX, baseZ, minY, maxY, maxX, maxZ, dx, dy;
	public float debug;
	public bool isTopDown;
	public bool isDragging;
	public Vector3 oldPos;
	public Vector2 dragSpeed;
	private Vector3 mouseLocation;
	void Start () {
		transform.rotation = Quaternion.Euler(45,-45,0);
		//base height
		baseY = 15f;
		//max height
		maxY = 25f;
		//min height
		minY = baseY;
		//Camera angle controller
		isTopDown = false;
		//Are we dragging camera?
		isDragging = false;
		dragSpeed.x = 5;
		dragSpeed.y = 5;

	}
	
	// Update is called once per frame
	void Update () {
		//Scroll wheel forward? Go up.
		if(Input.GetAxis ("Mouse ScrollWheel") < 0){
			baseY += .1f;
			//So it won't go too high
			if(baseY > maxY){
				baseY = maxY;
			}
		}
		//Scroll wheel back? Go down.
		if(Input.GetAxis ("Mouse ScrollWheel") > 0 ){
			baseY -= .1f;
			//So it won't go too low
			if(baseY < minY){
				baseY = minY;
			}
		}

		//Do we switch the view?
		if (Input.GetButtonDown ("SwitchView")) {
			if(isTopDown) {
				isTopDown = false;
				transform.rotation = Quaternion.Euler(45,-45,0);
			}
			else {
				isTopDown = true;
				transform.rotation = Quaternion.Euler(90,0,0);
			}
		}

		if (Input.GetMouseButton (0)) {
			dx = Input.GetAxis("Mouse X") * dragSpeed.x;
			dy = Input.GetAxis("Mouse Y") * dragSpeed.y;
			transform.position -= transform.right * dx + transform.up * dy;
			baseX = transform.position.x;
			baseY = transform.position.y;
			baseZ = transform.position.z;

			if (baseY < minY) {
				baseY = minY;
			}

			if (baseY > maxY) {
				baseY = maxY;
			}
		}

		//Move the camera based on current perspective
		//Move forward
		if (Input.GetKey (KeyCode.UpArrow)) {
			if (isTopDown) {
					baseZ += .1f; 
			} else {
					baseZ += .1f;
					baseX -= .1f;
			}
		}

		//Move backward
		if (Input.GetKey (KeyCode.DownArrow)) {
			if (isTopDown) {
					baseZ -= .1f;
			} else {
					baseZ -= .1f;
					baseX += .1f;

			}
		}

		//Move left
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if (isTopDown) {
					baseX -= .1f;
			} else {
					baseZ -= .1f;
					baseX -= .1f;
			}
		}

		//Move right
		if (Input.GetKey (KeyCode.RightArrow)) {
			if (isTopDown) {
					baseX += .1f;
			} else {
					baseZ += .1f; 
					baseX += .1f;
			}
		}

		//Update camera position.
		transform.position = new Vector3 (baseX, baseY, baseZ);
				
	}
}
