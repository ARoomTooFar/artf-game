using UnityEngine;
using System.Collections;

public class PlayerMarkerController : MonoBehaviour {
	Transform initPos;
	public string name;
	Vector3 playerPos;
	GameObject player;
	void Start () {
		initPos = transform;
		//Find should only ever run in start
		if (GameObject.Find (name) != null) {
			player = GameObject.Find (name);
			playerPos = player.transform.position;
		} else {//Fail check and removal
			gameObject.GetComponent<Renderer>().enabled = false;
		}
	}
	

	void Update () {
		if (playerPos != null) {
			playerPos = player.transform.position;
			playerPos.y = transform.position.y;
			transform.position = playerPos;
		}
	}
}
