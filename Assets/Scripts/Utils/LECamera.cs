using UnityEngine;
using System.Collections;

enum cam_views {play, top}; // rot90, rot180, rot270};

public class LECamera : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	public GameObject tilemap;
	Transform topdown;

	Vector3 offset;
	
	void Start () {
//		Camera.main.orthographic = true;
//		camera.orthographicSize = 30;
		target = tilemap.transform;
		topview ();
	}

	void topview(){
		transform.rotation = Quaternion.Euler (90, 0, 0);
		transform.LookAt (target);
		transform.Translate(new Vector3(12, 12, -20));
	}


	void FixedUpdate () {

	}
}
