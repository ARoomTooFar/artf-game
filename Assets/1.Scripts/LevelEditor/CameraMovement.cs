using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class CameraMovement : MonoBehaviour {
	private Camera mainCam;
	private CameraRaycast camCast;
	private float orthoZoomSpeed = 2f;
	private float maxOrthoSize = 30;
	private float minOrthoSize = 6;
	private float maxY = 50;
	private float minY = 10;
	private Vector3 prevMouse = Global.nullVector3;
	private Ray ray;
	Button btn;
	Sprite orth;
	Sprite pers;

	void Start() {
		// This should only ever be in the Level Editor, so I'm sticking a thing here to tell
		// the resource pool thing to strip colliders from game objects.
		Global.inLevelEditor = true;

		mainCam = Camera.main;
		camCast = mainCam.GetComponent<CameraRaycast>();

		foreach(Camera cam in Camera.allCameras) {
			cam.transform.position = Global.initCameraPosition;
			cam.transform.eulerAngles = Global.initCameraRotation;
		}

		btn = GameObject.Find("Button_CameraToggle").GetComponent("Button") as Button;
		orth = Resources.Load <Sprite>("LevelEditorIcons/orthog");
		pers = Resources.Load <Sprite>("LevelEditorIcons/perspe");
	}
	
	void Update() {
		checkForMouseClicks();
	}
	

	void checkForMouseClicks() {
		if(Input.GetMouseButton(1)) {
			dragCamera();
		} else {
			prevMouse = Global.nullVector3;
		}
	}
	
	public void dragCamera() {
		Vector3 point = camCast.mouseGroundPoint.Round(1);

		if(prevMouse == Global.nullVector3) {
			prevMouse = point;
		}
		
		Vector3 offset = (prevMouse - point);
		moveCamera(offset);	
	}
	
	public void moveForward() {
		float x = Mathf.Sin(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = Mathf.Cos(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void moveBackward() {
		float x = -Mathf.Sin(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = -Mathf.Cos(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void moveLeft() {
		float x = -Mathf.Cos(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = Mathf.Sin(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void moveRight() {
		float x = Mathf.Cos(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = -Mathf.Sin(mainCam.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void zoomCamIn() {
		if(mainCam.orthographic) {
			mainCam.orthographicSize = Mathf.Min(maxOrthoSize, mainCam.orthographicSize + orthoZoomSpeed);
			foreach(Camera cam in Camera.allCameras) {
				cam.orthographicSize = mainCam.orthographicSize;
			}
		} else {
			Vector3 old = mainCam.transform.position;
			moveCamera(-mainCam.transform.forward, mainCam.transform.position.y / 10f);
			if(mainCam.transform.position.y > maxY) {
				mainCam.transform.position = old;
			}
		}
	}

	public void zoomCamOut() {
		if(mainCam.orthographic) {
			mainCam.orthographicSize = Mathf.Max(minOrthoSize, mainCam.orthographicSize - orthoZoomSpeed);
			foreach(Camera cam in Camera.allCameras) {
				cam.orthographicSize = mainCam.orthographicSize;
			}
		} else {
			Vector3 old = mainCam.transform.position;
			moveCamera(mainCam.transform.forward, mainCam.transform.position.y / 10f);
			if(mainCam.transform.position.y < minY) {
				mainCam.transform.position = old;
			}
		}
	}
	
	void moveCamera(Vector3 pos, float speed = 1) {
		Vector3 vec = mainCam.transform.position;
		vec.x = vec.x + (pos.x * speed);
		vec.y = vec.y + (pos.y * speed);
		vec.z = vec.z + (pos.z * speed);
		mainCam.transform.position = vec;
	}
	
	public void changeToTopDown() {
		Vector3 oldFocus = getFocusPoint();
		mainCam.transform.eulerAngles = new Vector3(90, 0, 0);
		mainCam.orthographic = true;
		mainCam.orthographicSize = minOrthoSize + (maxOrthoSize-minOrthoSize) * Global.Normalize(mainCam.transform.position.y, minY, maxY);
		Vector3 newFocus = getFocusPoint();
		mainCam.transform.position = transform.position - newFocus + oldFocus;
		foreach(Camera cam in Camera.allCameras) {
			cam.orthographic = mainCam.orthographic;
			cam.orthographicSize = mainCam.orthographicSize;
		}
	}

	public void changeToPerspective() {
		// Get original focus point
		Vector3 oldFocus = getFocusPoint();
		// Change back to perspective and original rotation
		mainCam.orthographic = false;
		mainCam.transform.eulerAngles = Global.initCameraRotation;
		// Figure out the new Y value for zooming
		Vector3 newVec = Camera.main.transform.position;
		newVec.y = minY + (maxY-minY) * Global.Normalize(mainCam.orthographicSize, minOrthoSize, maxOrthoSize);
		Camera.main.transform.position = newVec;
		// Get the current focus point
		Vector3 newFocus = getFocusPoint(); 
		// Move camera to put focus back at the original focus point.
		transform.position = transform.position - newFocus + oldFocus;
		foreach(Camera cam in Camera.allCameras) {
			cam.orthographic = mainCam.orthographic;
		}
	}

	public void toggleCamera() {
		if(Camera.main.orthographic) {
			Camera.main.GetComponent<CameraMovement>().changeToPerspective();
			btn.GetComponent<Image>().sprite = pers;
		} else {
			Camera.main.GetComponent<CameraMovement>().changeToTopDown();
			btn.GetComponent<Image>().sprite = orth;
		}
	}

	public Vector3 getFocusPoint() {
		Camera UICamera = Camera.main;
		Ray ray = new Ray(UICamera.transform.position, UICamera.transform.forward);
		float distance;
		Global.ground.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}

	public void changeModes(){
		if(Mode.isRoomMode())
			Mode.setTileMode();
		else if(Mode.isTileMode())
			Mode.setRoomMode();
	}
}

