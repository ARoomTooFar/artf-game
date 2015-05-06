using UnityEngine;
using System.Collections;

public class PlayerMarkerController : MonoBehaviour {
	public string controllerName;
	Vector3 playerPos = Global.nullVector3;
	GameObject player;
	void Start () {
		//Find should only ever run in start
		if (GameObject.Find (controllerName) != null) {
			player = GameObject.Find (controllerName);
			playerPos = player.transform.position;
		} else {//Fail check and removal
			gameObject.GetComponent<Renderer>().enabled = false;
		}
	}
	

	void Update () {
		if (playerPos != Global.nullVector3) {
			playerPos = player.transform.position;
			playerPos.y = transform.position.y;
			transform.position = playerPos;
		}
	}
}
