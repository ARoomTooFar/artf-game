﻿using UnityEngine;
using System.Collections;

public class CooldownBar : LifeBar {

	public int active;
	public Material mat;
	public Material mat2;
	public Material mat3;
	void Start(){
		active = 0;
	}
	// Update is called once per frame
	void Update () {
		if(active==1){
			renderer.material = mat3;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
		}else if(active==2){
			renderer.material = mat2;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max,0, current)); 
		}else{
			renderer.material = mat;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, max, max)); 
		}
	}
}
