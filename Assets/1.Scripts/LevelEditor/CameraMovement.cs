using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class CameraMovement : MonoBehaviour {
	static Camera currentCamera;
	static float orthoZoomSpeed = 2f;
	static float maxOrthoSize = 30;
	static float minOrthoSize = 2;
	static float currOrthoSize = 10;
	static Vector3 cameraRotation = new Vector3(45, -45, 0);
	private Plane groundPlane = new Plane(Vector3.up, new Vector3());
	private Vector3 prevMouse = new Vector3();
	private bool prevMouseBool = false;
	private Ray ray;

	void Start() {
		
		// This should only ever be in the Level Editor, so I'm sticking a thing here to tell
		// the resource pool thing to strip colliders from game objects.
		GameObjectResourcePool.inLevelEditor = true;
		
		currentCamera = this.gameObject.GetComponent<Camera>();
		
		currentCamera.transform.position = new Vector3(43f, 15f, 2.5f);
		currentCamera.transform.eulerAngles = new Vector3(45, -45, 0);
		
		changeToPerspective();
	}
	
	void Update() {
		checkForMouseScrolling();
		checkForMouseClicks();
	}
	
	void checkForMouseScrolling() {
		if(Input.GetAxis("Mouse ScrollWheel") < 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			zoomCamIn();
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			zoomCamOut();
		}
	}
	
	void checkForMouseClicks() {
		if(Input.GetMouseButton(1)) {
			dragCamera();
		} else {
			prevMouseBool = false;
		}
	}
	
	public void dragCamera() {
		Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
		float distance = 0;
		groundPlane.Raycast(ray, out distance);
		Vector3 point = ray.GetPoint(distance).Round(1);
		
		if(!prevMouseBool) {
			prevMouse = point;
			prevMouseBool = true;
		}
		
		Vector3 offset = (prevMouse - point);
		moveCamera(offset);	
	}
	
	public void moveForward() {
		float x = Mathf.Sin(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = Mathf.Cos(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void moveBackward() {
		float x = -Mathf.Sin(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = -Mathf.Cos(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void moveLeft() {
		float x = -Mathf.Cos(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = Mathf.Sin(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void moveRight() {
		float x = Mathf.Cos(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		float z = -Mathf.Sin(currentCamera.transform.root.eulerAngles.y * Mathf.Deg2Rad);
		moveCamera(new Vector3(x, 0, z));
	}

	public void zoomCamIn() {
		if(currentCamera.orthographic) {
			currOrthoSize = Mathf.Min(maxOrthoSize, currOrthoSize + orthoZoomSpeed);
			currentCamera.orthographicSize = currOrthoSize;
		} else {
			//baseY = Mathf.Min (maxY, baseY + zoomSpeed);
			moveCamera(-currentCamera.transform.forward, currentCamera.transform.position.y / 10f);
		}
	}

	public void zoomCamOut() {
		if(currentCamera.orthographic) {
			currOrthoSize = Mathf.Max(minOrthoSize, currOrthoSize - orthoZoomSpeed);
			currentCamera.orthographicSize = currOrthoSize;
		} else {
			moveCamera(currentCamera.transform.forward, currentCamera.transform.position.y / 10f);
		}
	}
	
	void moveCamera(Vector3 pos, float speed = 1) {
		Vector3 vec = currentCamera.transform.position;
		vec.x = vec.x + (pos.x * speed);
		vec.y = vec.y + (pos.y * speed);
		vec.z = vec.z + (pos.z * speed);
		currentCamera.transform.position = vec;
	}
	
	public void changeToTopDown() {
		currentCamera.transform.eulerAngles = new Vector3(90, 0, 0);
		currentCamera.orthographic = true;
	}

	public void changeToPerspective() {
		currentCamera.orthographic = false;
		currentCamera.transform.eulerAngles = cameraRotation;
	}

	public void changetoOrthographic() {
		currentCamera.orthographic = true;
		currentCamera.transform.eulerAngles = cameraRotation;
	}
}

