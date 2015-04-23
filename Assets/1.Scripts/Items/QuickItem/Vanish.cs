using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vanish : QuickItem {

	public int vanDistance;
	public bool contact, gOut, activated;
	public float decSpeed, baseSize;
	public List<Character> enemies;
	public Stealth stealth;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
		cooldown = 30.0f;
		stealth = new Stealth();
		baseSize = cooldown*5;
		decSpeed = 1f;
		gOut = true;
		GetComponent<Collider>().enabled = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(activated){
			sizeAlter();
		}
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		activated = true;
		GetComponent<Renderer>().enabled = true;
		GetComponent<Collider>().enabled = true;
		user.BDS.addBuffDebuff(stealth,this.gameObject,baseSize*2+cooldown/2);
		
	}
	protected virtual void sizeAlter(){
		if(gOut){
			transform.localScale += new Vector3(decSpeed,0,decSpeed);
			if(transform.localScale.x >= baseSize || transform.localScale.z >= baseSize){
				gOut = false;
			}
		}else{
			transform.localScale -= new Vector3(decSpeed,0,decSpeed);
			if(transform.localScale.x < .5|| transform.localScale.z < .5){
				startSize();
				animDone();
			}
		}
	}
	protected virtual void startSize(){
		transform.localScale = new Vector3(0,.02f,0);
		activated = false;
		if(!contact){
			cooldown = cooldown*7.5f;
		}else{
			cooldown = 30f;
		}
		contact = false;
		gOut = true;
		GetComponent<Collider>().enabled = false;
		user.BDS.rmvBuffDebuff(stealth,this.gameObject);
		animDone();
	}
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	void OnTriggerEnter (Collider other) {
		// Will need a differentiation in the future(Or not if we want this)
		//     I suggest having the users know what is there enemy and settign ti that way somehow
		Enemy enemy = (Enemy) other.GetComponent(user.opposition);
		IForcible<Vector3, float> component = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>));
		if(component != null && enemy != null) {
			contact = true;
			enemies.Add (enemy);
			enemy.playerVanished(user.gameObject);
		}
	}
}
