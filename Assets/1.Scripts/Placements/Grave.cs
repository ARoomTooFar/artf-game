using UnityEngine;
using System.Collections;

public class Grave : MonoBehaviour {
	public Player unit;
	public Collider child;
	// Use this for initialization
	void Start () {
	
	}
	public void setInitValues(Player p){
		unit = p;
	}
	public void ressurrection(){
		child = this.GetComponentInChildren<Collider>();
		child.enabled = false;
		unit.rez();
		Destroy(gameObject);
	}
	// Update is called once per frame
	void Update () {
		/*
		if(Input.GetKey("space")){
			ressurrection();
		}*/
	}
}
