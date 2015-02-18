using UnityEngine;
using System.Collections;

public class CooldownBar : LifeBar {

	//public int onState;
	public Material mat;
	public Material mat2;
	public Material mat3;
	protected override void Start(){
		base.Start();
	}
	// Update is called once per frame
	protected override void Update () {
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		if(onState==1){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material = mat3;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
		}else if(onState==2){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material = mat2;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max,0, current)); 
		}else if(onState==3){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material = mat;
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0, max, max)); 
		}else{
			renderer.enabled = false;
			foreach (Renderer r in rs) {
				r.enabled = false;
			}
		}
	}
}
