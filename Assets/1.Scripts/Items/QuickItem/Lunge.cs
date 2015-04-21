using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lunge : QuickItem {

	//[Range(0, 10)]
	public float lungeDistance;
	public bool contact, gOut, activated;
	public float decSpeed, baseSize;
	public List<Character> enemies;
	public Immobilize immobil;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
		cooldown = 10.0f;
		immobil = new Immobilize();
		baseSize = cooldown*2;
		lungeDistance = baseSize;
		decSpeed = 1f;
		gOut = true;
		GetComponent<Collider>().enabled = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(activated){
			sizeAlter();
		}/*else{
			startSize();
		}*/
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		activated = true;
		GetComponent<Renderer>().enabled = true;
		GetComponent<Collider>().enabled = true;
		user.BDS.addBuffDebuff(immobil,this.gameObject);
		// player.animator.SetTrigger("Blink"); Once we have the animation for it
		//lungeFunc ();
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
				user.BDS.rmvBuffDebuff(immobil,this.gameObject);
				startSize();
				animDone();
			}
		}
	}
	protected virtual void startSize(){
		transform.localScale = new Vector3(0,.02f,0);
		activated = false;
		if(contact){
			cooldown = cooldown*7.5f;
		}else{
			cooldown = 10f;
		}
		contact = false;
		gOut = true;
		GetComponent<Collider>().enabled = false;
		user.BDS.rmvBuffDebuff(immobil,this.gameObject);
		//startSize();
		animDone();
	}
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	protected virtual void lungeFunc() {
		if(contact){
		RaycastHit hit;
		user.facing = enemies[0].transform.position - user.transform.position;
		float newDistance = lungeDistance;
		Vector3 newPosition;

		// Vector3 subFacing = user.facing.normalized * 0.2f;
		// user.transform.position = new Vector3(user.transform.position.x - subFacing.x, user.transform.position.y, user.transform.position.z - subFacing.z);
		// user.GetComponent<Rigidbody>().MovePosition(new Vector3(user.transform.position.x - subFacing.x, user.transform.position.y, user.transform.position.z - subFacing.z));

		// Check for obstacles in our way
		if (Physics.Raycast(user.transform.position, user.facing.normalized, out hit, lungeDistance)) {
			if (hit.transform.tag == "Wall"  || hit.collider.GetComponent(user.opposition) || hit.transform.tag == "Prop") {
				newDistance = hit.distance - 1f;
				print(hit.transform.name);
				//Debug.DrawLine(transform.position,hit.point,Color.red);
			}
		}
		//Debug.DrawLine(transform.position,hit.point,Color.red);

		/*
		if (user.GetComponent<Rigidbody>().SweepTest (user.facing, out hit, blinkDistance)) {
			print(hit.transform.name);
			if (hit.transform.tag == "Wall") {
				newDistance = hit.distance;
			}
		}*/
	
		newPosition = user.transform.position + user.facing.normalized * newDistance;
		
		while (!Physics.Linecast (newPosition, newPosition + Vector3.down * 5)) {
			newPosition = newPosition - user.facing.normalized * 0.5f;
		}

		user.transform.position = newPosition;
		// user.GetComponent<Rigidbody>().MovePosition(newPosition);

		animDone();
		GetComponent<Collider>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		user.BDS.rmvBuffDebuff(immobil,this.gameObject);
		user.gear.weapon.initAttack();
		contact = false;
		activated = false;
		}
		startSize();
	}
	void OnTriggerEnter (Collider other) {
		// Will need a differentiation in the future(Or not if we want this)
		//     I suggest having the users know what is there enemy and settign ti that way somehow
		Character enemy = (Character) other.GetComponent(user.opposition);
		IForcible<Vector3, float> component = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>));
		if(component != null && enemy != null) {
			contact = true;
			enemies.Add (enemy);
			print(enemy);
			lungeFunc();
		}
	}
}
