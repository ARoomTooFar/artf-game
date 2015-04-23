using UnityEngine;
using System.Collections;

public class Cloak : MonoBehaviour {
	public Material current,cloak;
	// Use this for initialization
	void Start () {
		current = GetComponent<Renderer>().material;
	}
	public void cloakSkin(){
		Material temp = current;
		current = cloak;
		cloak = temp;
		GetComponent<Renderer>().material = current;
		temp = null;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
