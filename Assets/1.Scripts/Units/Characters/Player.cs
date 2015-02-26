// Character class
// Base class that our heroes will inherit from

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Controls {
	//First 7 are Keys, last 2 are joystick axis
	public string up, down, left, right, attack, secItem, cycItem, hori, vert;
	//0 for Joystick off, 1 for Joystick on and no keys
	public int joyUsed;
}

[RequireComponent(typeof(Rigidbody))]
public class Player : Character, IMoveable {
	public bool inGrey;
	public int testDmg;
	public int greyDamage;
	public bool testable, isReady, atEnd, atStart;
	public GameObject currDoor;
	public GameObject expDeath;
	public Renderer[] rs;
	
	public UIActive UI;
	//public LifeBar hpBar, greyBar;
	//public AmmoBar ammoBar;
	//public List<CooldownBar> coolDowns = new List<CooldownBar>();
	public Controls controls;

	protected override void Awake() {
		base.Awake();
		opposition = Type.GetType("Enemy");
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
			animSteHash = animSteInfo.nameHash;

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
		/*if(Input.GetKey("space")&&testable){
				if(!stats.isDead){
				damage(testDmg);
				testable = false;
				}else{
					rez();
					testable=false;
				}
			}*/
		if (actable) {
			if(Input.GetKeyDown(controls.attack)) {
				if(currDoor!=null){
					currDoor.GetComponent<Door>().toggleOpen();
					currDoor = null;
				}else{
					animator.SetBool("Charging", true);
					gear.weapon.initAttack();
				}
			} else if(Input.GetKeyDown (controls.secItem)) {
				if (inventory.items.Count > 0 && inventory.items[inventory.selected].curCoolDown <= 0) {
					inventory.keepItemActive = true;
					inventory.items[inventory.selected].useItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
				} else {
					// Play sound for trying to use item on cooldown or items
					print("Item on Cooldown");
				}
			} else if(Input.GetKeyDown (controls.cycItem)) {
				inventory.cycItems();
			}
			// Continues with what is happening
		} else {
			
			if (!Input.GetKey(controls.attack)) {
				animator.SetBool ("Charging", false);
			}
			/*else if (animSteInfo.nameHash == rollHash) { for later
			}
			*/
		}
		
		
		if (Input.GetKeyUp (controls.secItem))  {
			if (inventory.items.Count > 0) {
				inventory.keepItemActive = false;
				// inventory.items[inventory.selected].deactivateItem(); // Item count check can be removed if charcters are required to have atleast 1 item at all times.
			}
		}
	}
	
	// Constant animation updates (Main loop for characters movement/actions)
	public override void animationUpdate() {
		if (attacking) {
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

		if (actable || (animator.GetBool("Charging") && (animSteHash == atkHashCharge || animSteHash == atkHashChgSwing))) {//gear.weapon.stats.curChgAtkTime > 0) { // Better Check here
			//"Up" key assign pressed
			if (Input.GetKey(controls.up)) {
				newMoveDir += Vector3.forward;
			}
			//"Down" key assign pressed
			if (Input.GetKey(controls.down)) {
				newMoveDir += Vector3.back;
			}
			//"Left" key assign pressed
			if (Input.GetKey(controls.left)) {
				newMoveDir += Vector3.left;
			}
			//"Right" key assign pressed
			if (Input.GetKey(controls.right)) {
				newMoveDir += Vector3.right;
			}
			//Joystick form
			if(controls.joyUsed == 1){
				newMoveDir = new Vector3(Input.GetAxis(controls.hori),0,Input.GetAxis(controls.vert));
			}
			
			// facing = newMoveDir;
			if (newMoveDir != Vector3.zero) {
				newMoveDir.y = 0.0f;
				facing = newMoveDir;
			}
			
			rigidbody.velocity = newMoveDir.normalized * stats.speed * stats.spdManip.speedPercent;
		} else if (freeAnim){
			// Right now this stops momentum when performing an action
			// If we trash the rigidbody later, we won't need this
			rigidbody.velocity = Vector3.zero;
		}
	}
	//-------------------------------------//

	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Character striker) {
		if (!invincible) {
			dmgTaken = Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(striker.transform.position, facing, transform.position)), 1, 100000);
		
			// print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			
			if (stats.health <= 0) {
				die();
			}
			UI.hpBar.current = stats.health;
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible) {
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);

			if (stats.health <= 0) {
				
				die();
			}
			UI.hpBar.current = stats.health;
		}
	}

	public override void die() {
		base.die();
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}
	}
	
	public override void heal(int healTaken){
		base.heal(healTaken);
		UI.hpBar.current = stats.health;
	}
	
	public override void rez(){
		base.rez();
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rs) {
			if(renderer.gameObject.tag != "Item")
			r.enabled = true;
		}
		UI.hpBar.current = stats.health;
	}

	//----------------------------------//

	// Grey Health functions
	public virtual int greyTest(int damage){
		if(((greyDamage + damage) > stats.health) && ((greyDamage + damage) < stats.maxHealth)){
			stats.health = 0;
			stats.isDead = true;
			return 0;
		}
		if(((greyDamage + damage) >= stats.maxHealth) && stats.health == stats.maxHealth){
			stats.health = 1;
			greyDamage = stats.maxHealth - 1;
			inGrey = true;
			return 0;
		}		
		if(damage > (stats.maxHealth/20)){
			//print("Got Here"+(stats.maxHealth/20)+":"+damage);
			int tempDmg = greyDamage;
			if(inGrey){
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+tempDmg);
				inGrey = true;
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				//print("True!WGAT:"+(stats.maxHealth/20 + tempDmg));
				return damage + tempDmg;
			}else{
				inGrey = true;
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+(damage - stats.maxHealth/20));
				//print("True!NGAT:"+stats.maxHealth/20);
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				return damage;
			}
		}
		if(inGrey){
			inGrey = false;
			//print("True!WGBT:"+(damage + greyDamage));
			return damage + greyDamage;
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
			UI.hpBar.current = stats.health;
			UI.greyBar.current = greyDamage+stats.health;
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
	//----------------------------------//
}