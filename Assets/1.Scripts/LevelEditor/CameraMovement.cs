using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class CameraMovement : MonoBehaviour {
	static Camera mainCam;
	static float orthoZoomSpeed = 2f;
	static float maxOrthoSize = 30;
	static float minOrthoSize = 2;

	private float maxY = 50;
	private float minY = 10;

	private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
	private Vector3 prevMouse = Global.nullVector3;
	private Ray ray;

	void Start() {
		
		// This should only ever be in the Level Editor, so I'm sticking a thing here to tell
		// the resource pool thing to strip colliders from game objects.
		Global.inLevelEditor = true;

		mainCam = Camera.main;

		foreach(Camera cam in Camera.allCameras) {
			cam.transform.position = Global.initCameraPosition;
			cam.transform.eulerAngles = Global.initCameraRotation;
		}
		
		changeToPerspective();
	}
	
	void Update() {
		checkForMouseScrolling();
		checkForMouseClicks();
	}
	
	void checkForMouseScrolling() {
		if(Input.GetAxis("Mouse ScrollWheel") < 0 /*&& UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false*/) {
			zoomCamIn();
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0 /*&& UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false*/) {
			zoomCamOut();
		}
	}
	
	void checkForMouseClicks() {
		if(Input.GetMouseButton(1)) {
			dragCamera();
		} else {
			prevMouse = Global.nullVector3;
		}
	}
	
	public void dragCamera() {
		Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
		float distance = 0;
		groundPlane.Raycast(ray, out distance);
		Vector3 point = ray.GetPoint(distance).Round(1);
		
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
			if(mainCam.transform.position.y > maxY){
				return;
			}
			moveCamera(-mainCam.transform.forward, mainCam.transform.position.y / 10f);
		}
	}

	public void zoomCamOut() {
		if(mainCam.orthographic) {
			mainCam.orthographicSize = Mathf.Max(minOrthoSize, mainCam.orthographicSize - orthoZoomSpeed);
			foreach(Camera cam in Camera.allCameras) {
				cam.orthographicSize = mainCam.orthographicSize;
			}
		} else {
			if(mainCam.transform.position.y < minY){
				return;
			}
			moveCamera(mainCam.transform.forward, mainCam.transform.position.y / 10f);
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
		foreach(Camera cam in Camera.allCameras) {
			cam.transform.eulerAngles = new Vector3(90, 0, 0);
			cam.orthographic = true;
		}
	}

	public void changeToPerspective() {
		foreach(Camera cam in Camera.allCameras) {
			cam.orthographic = false;
			cam.transform.eulerAngles = Global.initCameraRotation;
		}
	}

	public void changetoOrthographic() {
		foreach(Camera cam in Camera.allCameras) {
			cam.orthographic = true;
			cam.transform.eulerAngles = Global.initCameraRotation;
		}
	}
}

