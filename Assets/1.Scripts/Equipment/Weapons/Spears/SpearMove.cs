// Use for spear movment while we don't have animations

using UnityEngine;
using System.Collections;

public class SpearMove : MonoBehaviour {

	public Transform refThrust;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (refThrust.position.x, refThrust.position.y, refThrust.position.z);
	}
}
