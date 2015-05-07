// Character class
// Base class that our heroes will inherit from

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Controls {
	//First 7 are Keys, last 2 are joystick axis
	public string up, down, left, right, attack, secItem, cycItem, hori, vert, joyAttack, joySecItem, joyCycItem;
	//0 for Joystick off, 1 for Joystick on and no keys
	public bool joyUsed;
}

[RequireComponent(typeof(Rigidbody))]
public class NewPlayer : NewCharacter, IHealable<int>{
	public string nameTag;
	public int greyDamage;
	public bool testable, isReady, atEnd, atStart;
	
	public UIActive UI;
	public Controls controls;

	private Door currDoor;
	
	protected override void Awake() {
		base.Awake();
		//opposition = Type.GetType("NewEnemy");
		opposition = Type.GetType("NewEnemy"); //Use this if going after testable opponents
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=10;
		greyDamage = 0;
	}
	//Set cooldown bars to current items. 
	void ItemCooldowns() {
		UI.onState = true;
		UI.hpBar.onState =1;
		UI.greyBar.onState =1;
		for(int i = 0; i < inventory.items.Count; i++){
			UI.coolDowns[i].onState = 3;
			inventory.items[i].cdBar=UI.coolDowns[i];
			//inventory.items[i].cdBar = UI.getComponent("LifeBar");//coolDowns[i];
		}
		if(gear.weapon is RangedWeapons){
			gear.weapon.GetComponent<RangedWeapons>().loadData(UI.ammoBar);
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(isDead) return;

		if(UI!=null){
			if(UI.onState){
				ItemCooldowns();
				UI.hpBar.max = stats.maxHealth;
				UI.greyBar.max = stats.maxHealth;
				UI.greyBar.current = stats.health+greyDamage;
				UI.hpBar.current = stats.health;
			}
		}

		freeAnim = !stunned && !knockedback && !animationLock;
		actable = freeAnim;

		this.animator.SetBool("Actable", this.actable);

		if (!actable) return;
		actionCommands ();
		moveCommands ();
		animationUpdate ();
	}
	
	//---------------------------------//
	// Action interface implementation //
	//---------------------------------//
	
	public override void actionCommands() {
		// Invokes an action/animation
		if (actable) {
			if(Input.GetKeyDown(controls.attack) || Input.GetButtonDown(controls.joyAttack)) {
				if(currDoor!=null){
					currDoor.GetComponent<Door>().toggleOpen();
					currDoor = null;
				}
				animator.SetBool("Charging", true);
				gear.weapon.initAttack();
			} else if(Input.GetKeyDown (controls.secItem) || Input.GetButtonDown(controls.joySecItem)) {
				if (inventory.items.Count > 0 && inventory.items[inventory.selected].curCoolDown <= 0) {
					inventory.keepItemActive = true;
					inventory.items[inventory.selected].useItem(); // Item count check can be removed if characters are required to have atleast 1 item at all times.
				} else {
					// Play sound for trying to use item on cooldown or items
					print("Item on Cooldown");
				}
			} else if(Input.GetKeyDown (controls.cycItem) || Input.GetButtonDown(controls.joyCycItem)) {
				inventory.cycItems();
			}
			// Continues with what is happening
		} else {
			
			if (!Input.GetKey(controls.attack) && (!Input.GetButton(controls.joyAttack))) {
				animator.SetBool ("Charging", false);
			}
		}
		
		
		if (Input.GetKeyUp (controls.secItem) || Input.GetButtonUp(controls.joySecItem))  {
			if (inventory.items.Count > 0) {
				inventory.keepItemActive = false;
				// inventory.items[inventory.selected].deactivateItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
			}
		}
	}
	
	// Constant animation updates (Main loop for characters movement/actions, sets important parameters in the animator)
	public override void animationUpdate() {
		movementAnimation();
	}
	
	//-------------------------------------------//
	
	//-----------------------------------//
	// Movement interface implementation //
	//-----------------------------------//
	
	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;
		Vector3 camAngle = Camera.main.transform.eulerAngles;

		if (actable || animator.GetBool("Charging")) {
			float x;
			float z;
			if (Input.GetKey(controls.up) || Input.GetAxis(controls.vert) > 0) {
				x = Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
				z = Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
				newMoveDir += new Vector3(x, 0, z);
			}
			//"Down" key assign pressed
			if (Input.GetKey(controls.down) || Input.GetAxis(controls.vert) < 0) {
				x = -Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
				z = -Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
				newMoveDir += new Vector3(x, 0, z);
			}
			//"Left" key assign pressed
			if (Input.GetKey(controls.left) || Input.GetAxis(controls.hori) < 0) {
				x = -Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
				z = Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
				newMoveDir += new Vector3(x, 0, z);
			}
			//"Right" key assign pressed
			if (Input.GetKey(controls.right)|| Input.GetAxis(controls.hori) > 0) {
				x = Mathf.Cos(camAngle.y * Mathf.Deg2Rad);
				z = -Mathf.Sin(camAngle.y * Mathf.Deg2Rad);
				newMoveDir += new Vector3(x, 0, z);
			}
			
			// facing = newMoveDir;
			if (newMoveDir != Vector3.zero) {
				newMoveDir.y = 0.0f;
				facing = newMoveDir;
			}
			
			this.rb.velocity = newMoveDir.normalized * stats.speed * stats.spdManip.speedPercent;
		} else {
			// Right now this stops momentum when performing an action
			// If we trash the rigidbody later, we won't need this
			this.rb.velocity = Vector3.zero;
		}
	}
	//-------------------------------------//
	
	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		this.damage (dmgTaken, atkPosition);
	}
	
	public override void damage(int dmgTaken, Transform atkPosition) {
		if (invincible || stats.isDead) return;
		
		dmgTaken = Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(atkPosition.position, facing, transform.position)), 1, 100000);
		stats.health -= greyTest(dmgTaken);
		if (stats.health <= 0) die();
		
		// Blood Effect Code
		if(splatter == null) return;	
		splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position-new Vector3(0,.5f,0), Quaternion.identity)).GetComponent<splatCore>();
		theSplat.adjuster = (float) dmgTaken/stats.maxHealth;
		Destroy (theSplat, 2);
		
		hitConfirm = new Knockback(gameObject.transform.position-atkPosition.position,(float) dmgTaken/stats.maxHealth*25.0f);
		BDS.addBuffDebuff(hitConfirm,gameObject,.5f);
		
	}
	
	public override void damage(int dmgTaken) {
		if (invincible || stats.isDead) return;
		
		stats.health -= greyTest(dmgTaken);
		if (stats.health <= 0) this.die();
		
		if(splatter == null) return;
		splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position-new Vector3(0,.5f,0), Quaternion.identity)).GetComponent<splatCore>();
		theSplat.adjuster = (float) ((stats.maxHealth-stats.health)/stats.maxHealth);
		Destroy (theSplat, 2);
	}
	
	public override void die() {
		base.die();

		stats.health = 0;
		this.greyDamage = 0;
		if(UI!=null) UI.hpBar.current = 0;

		Renderer[] rs = GetComponentsInChildren<Renderer>();
		//GetComponent<Collider> ().isTrigger = true;
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}
	}
	
	//---------------------------------//
	
	//---------------------------------//
	// Heal Interface Implementation //
	//---------------------------------//
	public override void heal(int healTaken){
		if(stats.health < stats.maxHealth){
			stats.health+=healTaken;
			if(stats.health > stats.maxHealth){
				stats.health = stats.maxHealth;
			}
		}
		//UI.hpBar.current = stats.health;
		if(greyDamage > 0){
			greyDamage--;
			UI.greyBar.current = stats.health + greyDamage;
		}
	}
	
	public override void rez(){
		if(stats.isDead){
			stats.isDead = false;
			stats.health = stats.maxHealth/(2+2*stats.rezCount);
			// if(UI!=null) UI.hpBar.current = stats.health;
			stats.rezCount++;
		}
		//GetComponent<Collider> ().isTrigger = false;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rs) {
			if(r.GetComponent<Renderer>().gameObject.tag != "Item")
				r.enabled = true;
		}
	}
	
	//----------------------------------//

	//-------------//
	// Grey Health //
	//-------------//
	
	// Grey Health functions
	private int greyTest(int damage){
		// If grey damage plus damage kills, we kill them off
		if((greyDamage + damage) > stats.health) {
			StopCoroutine("RegenGrey");
			return damage + greyDamage;
		}
		
		int tempDmg = greyDamage;
		StopCoroutine("RegenGrey");
		
		if (damage > (stats.maxHealth/5)) {
			greyDamage = damage - stats.maxHealth/5;
			StartCoroutine("RegenGrey");
		} else {
			greyDamage = 0;
		}
		
		return damage + tempDmg;
	}

	private IEnumerator RegenGrey(){
		while (greyDamage > 0) {
			yield return new WaitForSeconds(1);
			this.stats.health++;
			this.greyDamage--;
		}
		yield return 0;
	}

	//---------------//

	//-------//
	// Doors //
	//-------//

	private void OnTriggerEnter(Collider other){
		if(other.tag == "Door"){
			currDoor = other.GetComponent<Door>();
		}
	}
	
	private void OnTriggerExit(Collider other){
		if(this.currDoor != null && other.gameObject == this.currDoor.gameObject){
			currDoor = null;
		}
	}
	//----------------------------------//
}