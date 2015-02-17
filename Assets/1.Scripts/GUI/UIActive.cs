using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIActive : MonoBehaviour {
	public bool onState;
	public LifeBar hpBar, greyBar;
	public AmmoBar ammoBar;
	public List<CooldownBar> coolDowns = new List<CooldownBar>();
	// Use this for initialization
	void Start () {
		//coolDowns.Count=3;
	}
	
	// Update is called once per frame
	void Update () {
		if(!onState){
			hpBar.onState = 0;
			greyBar.onState = 0;
			ammoBar.onState = 0;
			coolDowns[0].onState = 0;
			coolDowns[1].onState = 0;
			coolDowns[2].onState = 0;
		}
	}
	void beginBars(){
		
	}
}
