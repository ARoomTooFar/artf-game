using UnityEngine;
using System.Collections;

public class pan : MonoBehaviour {

	public GameObject target;
	public float radius = 10f;
	public float turnSpeed = 0.01f;
	public float ang;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		target.transform.position = offset;
		ang += Time.deltaTime * turnSpeed;
		Vector3 newPos = new Vector3 (target.transform.position.x + Mathf.Sin(ang)*radius,
		                              transform.position.y,
		                              target.transform.position.z + Mathf.Cos (ang)*radius);
		transform.position = newPos;
		transform.LookAt (target.transform);
	}
}
