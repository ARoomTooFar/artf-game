using UnityEngine;
using System.Collections;

//This class controls the level editor camera, allowing it
//to switch between ortho/top down perspectives, as well
//as zoom in/out, and move along the x and z axis using
//the arrow keys.
public class CameraAdjuster : MonoBehaviour {
	//Base location for camera, limits of camera,
	//boolean control for camera angle, and speed of
	//camera movement initialization maxX and maxY
	//tbd based on needs ot level editor, left blank here
	public float baseY,baseX,baseZ,adjVal,supBaseY, maxY, maxX, maxZ;
	public bool isTopDown;
	void Start () {
		transform.rotation = Quaternion.Euler(45,-45,0);
		//base height
		baseY = 15f;
		//max height
		maxY = 25f;
		//Base case reminder
		supBaseY = baseY;
		//Camera angle controller
		isTopDown = false; 

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
			if(baseY < supBaseY){
				baseY = supBaseY;
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

		//Move the camera based on current perspective
		//Move forward
		if (Input.GetKey (KeyCode.UpArrow)) {
			if(isTopDown) {
				baseZ += .1f; 
			}
			else {
				baseZ += .1f;
				baseX -= .1f;
			}
		}
		//Move backward
		if (Input.GetKey (KeyCode.DownArrow)) {
			if(isTopDown) {
				baseZ -= .1f;
			}
			else {
				baseZ -= .1f;
				baseX += .1f;

			}
		}
		//Move left
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if(isTopDown) {
				baseX -= .1f;
			}
			else {
				baseZ -= .1f;
				baseX -= .1f;
			}
		}
		//Move right
		if (Input.GetKey (KeyCode.RightArrow)) {
			if(isTopDown) {
				baseX += .1f;
			}
			else {
				baseZ += .1f; 
				baseX += .1f;
			}
		}

		//Update camera position.
		transform.position = new Vector3(baseX, baseY, baseZ);
		
	}
}
