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
	
	public void dragCamera ()
	{
		dx = Input.GetAxis ("Mouse X") * dragSpeed.x;
		dy = Input.GetAxis ("Mouse Y") * dragSpeed.y;
		UICamera.transform.position -= UICamera.transform.right * dx + UICamera.transform.up * dy;
		baseX = UICamera.transform.position.x;
		baseZ = UICamera.transform.position.z;
		
		if (baseY < minY) {
			baseY = minY;
		}
		
		if (baseY > maxY) {
			baseY = maxY;
		}
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
	}
	
	Vector3 getCameraForward(){
		return UICamera.transform.forward;
	}
	
	public void moveForward ()
	{
		//	if (isTopDown) {
		//		baseZ += .1f; 
		//		baseX -= .1f;
		//	} else {
		baseZ += .1f;
		baseX -= .1f;
		//	}
	}
	
	public void moveBackward ()
	{
		//	if (isTopDown) {
		//		baseZ -= .1f;
		//		baseX += .1f;
		//	} else {
		baseZ -= .1f;
		baseX += .1f;
		//	}
	}
	
	public void moveLeft ()
	{
		//	if (isTopDown) {
		//		baseX -= .1f;
		//		baseZ -= .1f;
		//	} else {
		baseZ -= .1f;
		baseX -= .1f;
		//	}
	}
	
	public void moveRight ()
	{
		//	if (isTopDown) {
		//		baseX += .1f;
		//		baseZ += .1f;
		//	} else {
		baseZ += .1f; 
		baseX += .1f;
		//}
	}
	
	public void zoomCamIn ()
	{
		baseY += zoomSpeed;
		if (baseY > maxY) {
			baseY = maxY;
		}
	}
	
	public void zoomCamOut ()
	{
		baseY -= zoomSpeed;
		if (baseY < minY) {
			baseY = minY;
		}
	}
	
	public void changeToTopDown ()
	{
		setCameraRotation (new Vector3 (90, -45, 0));
		isTopDown = true;
	}
	
	public void changeToPerspective ()
	{
		UICamera.orthographic = false;
		OnTopCamera.orthographic = false;
		isTopDown = false;
		
		setCameraRotation (new Vector3 (45, -45, 0));
	}
	
	public void changetoOrthographic ()
	{
		UICamera.orthographic = true;
		OnTopCamera.orthographic = true;
		isTopDown = false;
		
		setCameraRotation (new Vector3 (45, -45, 0));
	}
}
