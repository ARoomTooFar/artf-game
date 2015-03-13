using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class Output_Camera : MonoBehaviour
{
	static Camera UICamera;
	static Camera OnTopCamera;
	
	static float baseX = 43f;
	static float baseY = 15f;
	static float baseZ = 2.5f;
	static float minY = 5f;
	static float maxY = 25f;
	static Vector2 dragSpeed;
	//	static float scrollSpeed = 2f;
	static float zoomSpeed = 2f;
	static bool isTopDown = false;
	float dx;
	float dy;
	
	void Start ()
	{
		dragSpeed = new Vector2 (3f, 3f);
		
		UICamera = GameObject.Find ("UICamera").camera;
		OnTopCamera = GameObject.Find ("LayersOnTopOfEverythingCamera").camera;
		
		setCameraRotation (new Vector3 (45, -45, 0));
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
		
		changeToPerspective ();
	}
	
	void Update ()
	{
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
	}
	
	public void cursorToHand(){
		
	}
	
	public void cursorToPointer(){
		
	}
	
	void setCameraRotation (Vector3 rot)
	{
		UICamera.transform.rotation = Quaternion.Euler (rot);
	}
	
	void setCameraPosition (Vector3 pos)
	{
		UICamera.transform.position = pos;
	}
	
	public Vector3 dragCamera (Vector3 oldPos, Vector3 delta)
	{
		//delta = new Vector3 (2, 2, 2);
		delta = Input.mousePosition - oldPos; //Base the drag speed of the change in mouse position in time.
		dragSpeed = new Vector2(delta.x, delta.y); //Set drag speed
		dx = Input.GetAxis ("Mouse X") * dragSpeed.x;
		dy = Input.GetAxis ("Mouse Y") * dragSpeed.y;
		UICamera.transform.position -= UICamera.transform.right * dx + UICamera.transform.up * dy;
		baseX = UICamera.transform.position.x; //Update position variables
		baseZ = UICamera.transform.position.z;

		//Make sure we aren't over or below our min/max y.
		if (baseY < minY) {
			baseY = minY;
		}
		
		if (baseY > maxY) {
			baseY = maxY;
		}
		//Update position and return.
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
		return oldPos = Input.mousePosition;
	}
	
	Vector3 getCameraForward(){
		return UICamera.transform.forward;
	}

	//Moving forward
	public void moveForward ()
	{
		baseZ += .1f;
		baseX -= .1f;
	}
	//Moving backward
	public void moveBackward ()
	{
		baseZ -= .1f;
		baseX += .1f;
	}
	//Moving left
	public void moveLeft ()
	{
		baseZ -= .1f;
		baseX -= .1f;
	}
	//Moving right
	public void moveRight ()
	{

		baseZ += .1f;
		baseX += .1f;
	}
	//Zoom in cam
	public void zoomCamIn ()
	{
		baseY += zoomSpeed;
		if (baseY > maxY) {
			baseY = maxY;
		}
	}
	//zoom out cam
	public void zoomCamOut ()
	{
		baseY -= zoomSpeed;
		if (baseY < minY) {
			baseY = minY;
		}
	}
	//Change to top down perspective forced
	public void changeToTopDown ()
	{
		UICamera.orthographic = false;
		OnTopCamera.orthographic = false;
		setCameraRotation (new Vector3 (90, -45, 0));
		//isTopDown = true;
	}
	//Change to perspective
	public void changeToPerspective ()
	{
		UICamera.orthographic = false;
		OnTopCamera.orthographic = false;
		//isTopDown = false;
		setCameraRotation (new Vector3 (45, -45, 0));
	}
	//Change to ortho.
	public void changetoOrthographic ()
	{
		UICamera.orthographic = true;
		OnTopCamera.orthographic = true;
	//	isTopDown = false;
		setCameraRotation (new Vector3 (45, -45, 0));
	}
}