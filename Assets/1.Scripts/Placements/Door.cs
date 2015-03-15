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
			GetComponent<Renderer>().enabled = false;
			stand.GetComponent<Renderer>().enabled = false;
			if(GetComponent<Collider>().enabled && !GetComponent<Collider>().isTrigger){
				//stand.collider.enabled = false;
				GetComponent<Collider>().enabled = false;
			}
		}
		else{
			open = false;
			show = true;
			GetComponent<Renderer>().enabled = true;
			stand.GetComponent<Renderer>().enabled = true;
			if(!GetComponent<Collider>().enabled && !GetComponent<Collider>().isTrigger){
				
				GetComponent<Collider>().enabled = true;
			}
		}
		//StopCoroutine("revWait");
		//StartCoroutine("revWait",disappear);
	}
	// Update is called once per frame
	protected override void Update () {
		if(open){
			stand.GetComponent<Collider>().enabled = false;
		}else{
			stand.GetComponent<Collider>().enabled = true;
		}
	}
}
