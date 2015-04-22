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
	
	public PlayerUI pui;


	protected virtual void Start(){
		onState = 0;
		//Renderer[] rs = GetComponentsInChildren<Renderer>();

		pui = GameObject.Find ("PlayerUI").GetComponent("PlayerUI") as PlayerUI;
	}
	protected virtual void Update () {
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		if(onState==1){
			GetComponent<Renderer>().enabled = true;
			foreach (Renderer r in rs) {
				r.enabled = true;
			}
			GetComponent<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(max, 0, current)); 
		}
		else{
			GetComponent<Renderer>().enabled = false;
			foreach (Renderer r in rs) {
				r.enabled = false;
			}
		}

	}



}
