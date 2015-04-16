using UnityEngine;
using System.Collections;

public class splatCast : MonoBehaviour {
	public GameObject splat;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				GameObject theSplat = (GameObject)Instantiate (splat, hit.point + (hit.normal * 2.5f), Quaternion.identity);
				Destroy (theSplat, 2);
			}
		}
	}
}
