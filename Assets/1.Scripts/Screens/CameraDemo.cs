using UnityEngine;
using System.Collections;

public class CameraDemo : MonoBehaviour {
	public bool moveTrigger = false;
	
	void Update () {
		if (moveTrigger) {
			if (transform.position.z <= -116) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 0.2f);
			}
		}
	}

	public void StartMove () {
		moveTrigger = true;
	}
}
