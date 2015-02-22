using UnityEngine;
using System.Collections;

public class Door : Wall {
	public bool open;
	// Use this for initialization
	protected override void Start () {
		base.Start();
		open = false;
	}
	protected override void Awake(){
		base.Awake();
	}
	public virtual void toggleOpen(){
		if(!open){
			open = true;
			show = false;
			renderer.enabled = false;
			stand.renderer.enabled = false;
			if(collider.enabled && !collider.isTrigger){
				//stand.collider.enabled = false;
				collider.enabled = false;
			}
		}
		else{
			open = false;
			show = true;
			renderer.enabled = true;
			stand.renderer.enabled = true;
			if(!collider.enabled && !collider.isTrigger){
				
				collider.enabled = true;
			}
		}
		//StopCoroutine("revWait");
		//StartCoroutine("revWait",disappear);
	}
	// Update is called once per frame
	protected override void Update () {
		if(open){
			stand.collider.enabled = false;
		}else{
			stand.collider.enabled = true;
		}
	}
}
