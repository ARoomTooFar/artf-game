using UnityEngine;
using System.Collections;

public class PlayerMarkerController : MonoBehaviour {
	Transform initPos;

	void Start () {
		initPos = transform;
	}
	

	void Update () {
		Vector3 playerPos = GameObject.Find ("PlayerZ").transform.position;
		playerPos.y = transform.position.y;

		transform.position = playerPos;
	}
}
