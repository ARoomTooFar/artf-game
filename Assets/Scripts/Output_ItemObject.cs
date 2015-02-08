using UnityEngine;
using System.Collections;

public class Output_ItemObject : MonoBehaviour {
//	GameObject itemObject; //object we're messing with
	Vector3 rotation = new Vector3(0f, 90f, 0f);
	Vector3 position;
	string name;

	void Update () {
		maintainOrientation();
		maintainPosition();
	}

	//position

	void maintainPosition(){
		transform.position = position;
	}

	public void changePosition(Vector3 newPos){
		position = newPos;
	}

	public Vector3 getPosition(){
		return position;
	}

	//rotation
	
	public void rotate(float deg){
		rotation.x = 0f;
		rotation.z = 0f;
		rotation.y += deg;
	}

	void maintainOrientation(){
		transform.eulerAngles = rotation;
	}

	public void changeOrientation(Vector3 newRot){
		transform.eulerAngles = newRot;
	}

	//name

	public string getName(){
		return name;
	}

	public void setName(string s){
		name = s;
	}

	public GameObject getGameObject(){
		//$
		return this.gameObject;
	}
}
