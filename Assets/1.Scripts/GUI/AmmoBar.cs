using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmmoBar : LifeBar {

	public int active;
	public Material mat;
	public Material mat2;
	void Start(){
		active = 0;
	}
	// Update is called once per frame
	void Update () {
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		if(active==1){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material = mat2;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
		}else if(active==2){
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
