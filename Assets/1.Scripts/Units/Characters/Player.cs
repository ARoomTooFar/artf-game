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
public class Player : Character, IMoveable, IHealable<int>{
	public string nameTag;
	public int testDmg;
	public int greyDamage;
	public bool testable, isReady, atEnd, atStart, inGrey;
	public GameObject currDoor;
	
	public UIActive UI;
	public Controls controls;
	
	// Events for death
	public delegate void DieBroadcast(GameObject dead);
	public static event DieBroadcast OnDeath;
	
	protected override void Awake() {
		base.Awake();
		//opposition = Type.GetType("NewEnemy");
		opposition = Type.GetType("Enemy"); //Use this if going after testable opponents
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=10;
		stats.luck=0;
		inGrey = false;
		greyDamage = 0;
		testDmg = 0;
		//testable = true;
		
	}
	//Set cooldown bars to current items. 
	void ItemCooldowns(){
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
		if(UI!=null){
			if(!UI.onState){
				ItemCooldowns();
			}
			//testable = false;
		}
		if(stats.health <= 0){
			
			isDead = true;
			if(UI!=null) UI.hpBar.current = 0;
		} else {
			if(UI!=null){
				if(UI.onState){
					UI.hpBar.max = stats.maxHealth;
					UI.greyBar.max = stats.maxHealth;
					UI.greyBar.current = stats.health+greyDamage;
					UI.hpBar.current = stats.health;
				}
			}
			isDead = false;
		}
		if(!isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
			
			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.fullPathHash;
			
			freeAnim = !stunned && !knockedback;
			
			
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd;
			
			if (isGrounded) {
				actionCommands ();
				moveCommands ();
			} else {
				falling();
			}
			
			animationUpdate ();
		}
	}
	
	//---------------------------------//
	// Action interface implementation //
	//---------------------------------//
	
	public override void actionCommands() {
		// Invokes an action/animation
		if(Input.GetKey("space")&&testable){
			if(!stats.isDead){
				damage(testDmg);
				testable = false;
			}/*else{
					rez();
					testable=false;
				}*/
		}
		if (actable) {
			if(Input.GetKeyDown(controls.attack) || Input.GetButtonDown(controls.joyAttack)) {
				if(currDoor!=null){
					currDoor.GetComponent<Door>().toggleOpen();
					currDoor = null;
					animator.SetBool("Charging", true);
					//Debug.Log(luckCheck());
					gear.weapon.initAttack();
				}else{
					animator.SetBool("Charging", true);
					//Debug.Log(luckCheck());
					gear.weapon.initAttack();
				}
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
			/*else if (animSteInfo.nameHash == rollHash) { for later
			}
			*/
		}
		
		
		if (Input.GetKeyUp (controls.secItem) || Input.GetButtonUp(controls.joySecItem))  {
			if (inventory.items.Count > 0) {
				inventory.keepItemActive = false;
				// inventory.items[inventory.selected].deactivateItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
			}
		}
	}
	
	// Constant animation updates (Main loop for characters movement/actions)
	public override void animationUpdate() {
		if (attacking&&!stats.isDead) {
			attackAnimation();
		} else {
			movementAnimation();
		}
	}
	
	//-------------------------------------------//
	
	//-----------------------------------//
	// Movement interface implementation //
	//-----------------------------------//
	
	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;
		Vector3 camAngle = Camera.main.transform.eulerAngles;

		
		if (!stats.isDead&&actable || (animator.GetBool("Charging") && (animSteHash == atkHashCharge || animSteHash == atkHashChgSwing) && this.testControl)) {//gear.weapon.stats.curChgAtkTime > 0) { // Better Check here
			//"Up" key assign pressed

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
		} else if (freeAnim){
			// Right now this stops momentum when performing an action
			// If we trash the rigidbody later, we won't need this
			this.rb.velocity = Vector3.zero;
		}
	}
	//-------------------------------------//
	
	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Character striker) {
		if (!invincible&&!stats.isDead) {
			dmgTaken = Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(striker.transform.position, facing, transform.position)), 1, 100000);
			if(splatter != null){
				splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position-new Vector3(0,.5f,0), Quaternion.identity)).GetComponent<splatCore>();
				theSplat.adjuster = (float) dmgTaken/stats.maxHealth;
				Destroy (theSplat, 2);
			}
			// print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			
			if (stats.health <= 0) {
				die();
			}
			//			UI.hpBar.current = stats.health;
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible&&!stats.isDead) {
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			if(splatter != null){
				splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position-new Vector3(0,.5f,0), Quaternion.identity)).GetComponent<splatCore>();
				theSplat.adjuster = (float) ((stats.maxHealth-stats.health)/stats.maxHealth);
				Destroy (theSplat, 2);
			}
			//UI.greyBar.current = greyDamage+stats.health;
			if (stats.health <= 0) {
				
				die();
			}
			//UI.hpBar.current = stats.health;
		}
	}
	
	public override void die() {
		Debug.Log("IsDead");
		base.die();
		if (OnDeath != null) {
			OnDeath (this.gameObject);
		}
		stats.health = 0;
		//UI.hpBar.current = 0;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position-new Vector3(0,6,0), transform.rotation)).GetComponent<Explosion>();
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
		}//prior heal base
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
			stats.rezCount++;
		}else{
			heal(stats.maxHealth/(2+2*stats.rezCount));
		}//if and else are the 'base' rez from prior
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rs) {
			if(GetComponent<Renderer>().gameObject.tag != "Item")
				r.enabled = true;
		}
		//UI.hpBar.current = stats.health;
	}
	
	//----------------------------------//
	
	// Grey Health functions
	public virtual int greyTest(int damage){
		if(((greyDamage + damage) > stats.health) && ((greyDamage + damage) < stats.maxHealth)){
			stats.health = 0;
			die();
			return 0;
		}
		if(((greyDamage + damage) >= stats.maxHealth) && stats.health == stats.maxHealth){
			stats.health = 1;
			greyDamage = stats.maxHealth - 1;
			inGrey = true;
			return 0;
		}
		if((damage > (stats.maxHealth/5)) && !inGrey){
			//print("Got Here"+(stats.maxHealth/20)+":"+damage);
			int tempDmg = greyDamage;
			if(inGrey){
				greyDamage = damage - stats.maxHealth/5;
				print("Grey!:"+tempDmg);
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				//print("True!WGAT:"+(stats.maxHealth/20 + tempDmg));
				return damage + tempDmg;
			}else{
				inGrey = true;
				greyDamage = damage - stats.maxHealth/5;
				//print("Grey!:"+(damage - stats.maxHealth/20));
				//print("True!NGAT:"+stats.maxHealth/20);
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				return damage;
			}
		}
		else if(inGrey && !(damage > (stats.maxHealth/5))){
			inGrey = false;
			int tempDmg = greyDamage;
			greyDamage = 0;
			//print("True!WGBT:"+(damage + greyDamage));
			return damage + tempDmg;
		}
		else{
			inGrey = false;
			//print("True!NG:"+damage);
			return damage;
		}
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
	}
	private IEnumerator RegenWait(){
		yield return new WaitForSeconds(1);
		if(inGrey && !stats.isDead){
			// print("Healed Grey and True");
			stats.health++;
			greyDamage--;
			//UI.hpBar.current = stats.health;
			//UI.greyBar.current = greyDamage+stats.health;
			if(greyDamage == 0){
				inGrey = false;
			}
			if(greyDamage > 0){
				StartCoroutine("RegenWait");
			}
		}
		yield return 0;
	}
	private void OnTriggerStay(Collider other){
		if(other.tag == "Door"){
			currDoor = other.gameObject;
		}
	}
	private void OnTriggerExit(Collider other){
		if(other.tag == "Door"){
			currDoor = null;
		}
	}
	//----------------------------------//
}