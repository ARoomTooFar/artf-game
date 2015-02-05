using UnityEngine;
using System.Collections;

//This class controls the level editor camera, allowing it
//to switch between ortho/top down perspectives, as well
//as zoom in/out, and move along the x and z axis using
//the arrow keys. Also movement along x and y axis
//via dragging.
public class TransformHandler_Camera : MonoBehaviour {
	//Base location for camera, limits of camera,
	//boolean control for camera angle, and speed of
	//camera movement, drag speed, and mouse location
	//initialization maxX and maxY tbd based on needs 
	//of level editor, left blank here
	float baseY, baseX, baseZ, minY, maxY, maxX, maxZ, dx, dy;
	bool isTopDown;
	bool isDragging;
	Vector2 dragSpeed;
	private Vector3 mouseLocation;
	private float scrollSpeed;
	Camera cam;

	void Start () {
		cam = GameObject.Find("UICamera").camera;
		cam.transform.rotation = Quaternion.Euler(45,45,0);
		cam.transform.position = new Vector3(-5f,21f,2.5f);
		//base height
		baseY = 15f;
		//max height
		maxY = 25f;
		//min height
		minY = 5f;
		//Camera angle controller
		isTopDown = false;
		//Are we dragging camera?
		isDragging = false;
//		dragSpeed.x = .5f;
//		dragSpeed.y = .5f;

		scrollSpeed = 2f;

	}

	public void zoomCamIn(float speed){
		baseY += speed;
		//So it won't go too high
		if(baseY > maxY){
			baseY = maxY;
		}
	}

	public void zoomCamOut(float speed){
		baseY -= speed;
		//So it won't go too low
		if(baseY < minY){
			baseY = minY;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Scroll wheel forward? Go up.
		if(Input.GetAxis ("Mouse ScrollWheel") < 0){
			zoomCamIn(scrollSpeed);
		}
		//Scroll wheel back? Go down.
		if(Input.GetAxis ("Mouse ScrollWheel") > 0 ){
			zoomCamOut(scrollSpeed);
		}

		//Do we switch the view?
		if (Input.GetButtonDown ("SwitchView")) {
			if(isTopDown) {
				isTopDown = false;
				cam.transform.rotation = Quaternion.Euler(45,-45,0);
			}
			else {
				isTopDown = true;
				cam.transform.rotation = Quaternion.Euler(90,0,0);
			}
		}

		//click and drag 
		//note for Jim or Leland: It took me 2 hours 
		//to realize I had to remove the baseY = transform.position.y
		//to get the desired effect...Please count that :(
		if (Input.GetMouseButton (1)) {
			dx = Input.GetAxis("Mouse X") * dragSpeed.x;
			dy = Input.GetAxis("Mouse Y") * dragSpeed.y;
			cam.transform.position -= cam.transform.right * dx + cam.transform.up * dy;
			baseX = cam.transform.position.x;
			baseZ = cam.transform.position.z;

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
		//		cam.transform.position = new Vector3 (baseX, baseY, baseZ);
				
	}
}
