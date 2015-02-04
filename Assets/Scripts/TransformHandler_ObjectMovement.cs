using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class attaches to objects, and lets the rotate
//by 90 degree increments, for when the user clicks the 
//rotation arrow.
public class TransformHandler_ObjectMovement : MonoBehaviour {
	public Button Button_Rotate = null; //holds the rotate button arrow
	Vector3 objectRotation = new Vector3(); //for holding this object's rotation
	public TileMap tileMap;

	void Start () {
		//Initialize the object's rotation to 90 degrees
		objectRotation = transform.rotation.eulerAngles;
		objectRotation.x = 0f;
		objectRotation.z = 0f;
		objectRotation.y = 90f;

		//we work in euler angles because Quaternions are tarded
		transform.rotation = Quaternion.Euler(objectRotation);

		//setup the listener for when the rotation button is clicked
		Button_Rotate.onClick.AddListener (() => {
			rotateObject (); });

	}

	//This function increments the object's rotation by 90 degrees
	public void rotateObject(){
		objectRotation = transform.rotation.eulerAngles;
		objectRotation.x = 0f;
		objectRotation.z = 0f;
		objectRotation.y += 90f;
	}

	public void snapObjectToGrid(Vector3 vec){

	}

	void Update () {
		//continually update the object's rotation, so it is locked in
		//position and doesn't spin around all crazy
		transform.rotation = Quaternion.Euler(objectRotation);
//		Vector3 pos = transform.position;
//		pos.y = 1f;
//		pos.x = Mathf.RoundToInt( pos.x  / tileMap.tileSize );
//		pos.z = Mathf.RoundToInt( pos.z  / tileMap.tileSize );
//		transform.position = pos;
	}
}
