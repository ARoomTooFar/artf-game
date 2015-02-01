using UnityEngine;
using System.Collections;

public class GrowthShot : MonoBehaviour {
	
	public Transform end;
	public Transform start;
	public bool active;
	public int power;
	public Character user;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(active){
			transform.position = Vector3.MoveTowards (transform.position, end.position, .1f);
			transform.localScale += new Vector3(0.02f,0.01f,0.02f);
		}
	}
	void OnTriggerEnter(Collider other){
		if (other.transform == end) {
		    collider.enabled = false;
			renderer.enabled = false;
			active = false; 
			transform.localScale = new Vector3(1,1,1);
			transform.position = start.position;
		}
	}
}
