﻿using UnityEngine;
using System.Collections;

public class splatCore : MonoBehaviour {
	public GameObject drip;
	public float adjuster;

	int drops;
	// Use this for initialization
	void Start () {
		//drops = (int) Random.Range(100,200);
	}
	void Awake(){
		RaycastHit hit;
		int x = -1;

		drops = Random.Range(50,100);
		while(x<=drops){
			x++;
			Vector3 fwd = transform.TransformDirection(Random.onUnitSphere*5);
			if(Physics.Raycast(transform.position, fwd, out hit, 9)){ 
				if(hit.collider.tag == "Floor" || hit.collider.tag == "Wall"){
				GameObject splatter = ((GameObject)Instantiate(drip,hit.point + (hit.normal *0.1f),Quaternion.FromToRotation(Vector3.up, hit.normal)));
				float scaler = Random.value;
                splatter.transform.localScale = new Vector3(splatter.transform.localScale.x*scaler,splatter.transform.localScale.y,splatter.transform.localScale.z*scaler);
                
                int rater = Random.Range (0, 180);
                splatter.transform.RotateAround (hit.point, hit.normal, rater);
                Destroy (splatter, 5);
				}else{
					drops--;
					x-=2;
				}
			}
		}
		if (x > drops) {
			Destroy (gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
