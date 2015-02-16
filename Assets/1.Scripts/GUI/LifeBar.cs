using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class controls the circle HP ring
public class LifeBar: MonoBehaviour
{
	public float max;
	public float current;
	public float onState;
	public Renderer[] rs;
	void Start(){
		onState = 0;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
	}
	void Update () {
		//Renderer[] rs = GetComponentsInChildren<Renderer>();
		if(onState==1){
			renderer.enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
		}
		else{
			renderer.enabled = false;
			foreach (Renderer r in rs) {
				r.enabled = false;
			}
		}
	}
}
