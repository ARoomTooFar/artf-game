using UnityEngine;
using System.Collections;

public class IndicatorFaceCam : MonoBehaviour {
	public Camera cam;
	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("PerspectiveAngledCamera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Canvas> ().transform.rotation = Quaternion.Euler (cam.transform.rotation.eulerAngles);
	}
}
