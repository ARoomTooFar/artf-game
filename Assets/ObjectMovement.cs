using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class attaches to objects, and helps control their rotation and position
public class ObjectMovement : MonoBehaviour {
	//Vector3 p = new Vector3();
	public Button Button_Rotate = null;

	Vector3 objectRotation = new Vector3();
	
	void Start () {
		objectRotation = transform.rotation.eulerAngles;
		objectRotation.x = 0f;
		objectRotation.z = 0f;
		objectRotation.y = 90f;
		transform.rotation = Quaternion.Euler(objectRotation);

		Button_Rotate.onClick.AddListener (() => {
			rotateObject (); });
	}

	public void rotateObject(){
		objectRotation = transform.rotation.eulerAngles;
		objectRotation.x = 0f;
		objectRotation.z = 0f;
		objectRotation.y += 90f;
		//transform.rotation = Quaternion.Euler(objectRotation);
	}
	

	void Update () {
		transform.rotation = Quaternion.Euler(objectRotation);
	}
}
