using UnityEngine;
using System.Collections;

//This class attaches to objects, and helps control their rotation and position
public class ObjectMovement : MonoBehaviour {
	Vector3 p = new Vector3();
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		p = transform.rotation.eulerAngles;
		p.x = 0f;
		p.z = 0f;
		transform.rotation = Quaternion.Euler(p);
	}
}
