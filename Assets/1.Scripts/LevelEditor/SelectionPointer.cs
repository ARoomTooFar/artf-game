using UnityEngine;
using System.Collections;

public class SelectionPointer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rot = transform.eulerAngles;
		rot.y += Time.deltaTime * 360;
		transform.eulerAngles = rot;
	}
}
