using UnityEngine;
using System.Collections;

public class LockToGridTest : MonoBehaviour {
	public Transform groundPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;

		pos.y = groundPosition.position.y + 0.5f;
//		pos.x = Mathf.Round (pos.x);
//		pos.z = Mathf.Round (pos.z);

		//Debug.Log (pos.x);


		transform.position = pos;


		Vector3 rot = new Vector3 (0, 0, 0);
		transform.rotation = Quaternion.Euler(rot);
	}
}
