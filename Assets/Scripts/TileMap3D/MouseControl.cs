using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour {
	
	void Start () {
		GetComponent<MeshCollider> ().sharedMesh = null;
		GetComponent<MeshCollider> ().sharedMesh = GetComponent<MeshFilter> ().mesh;
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
			renderer.material.color = Color.red;
		} else {
			renderer.material.color = Color.green;
		}
	}
}
