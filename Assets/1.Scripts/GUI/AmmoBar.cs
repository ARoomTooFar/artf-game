using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoBar : LifeBar {

	//public int onState;
	public Material mat;
	public Material mat2;
	void Start(){
		base.Start();
	}
	// Update is called once per frame
	void Update () {
		//Renderer[] rs = GetComponentsInChildren<Renderer>();
		if(onState==1){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material = mat2;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
		}else if(onState==2){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material = mat;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max,0, current)); 
		}else{
			renderer.enabled = false;
			foreach (Renderer r in rs) {
				r.enabled = false;
			}
		}
	}
}
