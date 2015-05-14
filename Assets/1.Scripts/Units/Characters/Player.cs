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
public class Player : NewCharacter, IHealable<int>{
	public string nameTag;
	public int greyDamage;
	public bool testable, isReady, atEnd, atStart, inGrey;
	public int mash_threshold;
	public int mash_value;
	public bool break_free;
	public bool tapped;
	public float last_pressed;

	public UIActive UI;
	public Controls controls;

	GameObject sparks = null;

	private Door currDoor;
	
	protected override void Awake() {
		base.Awake();
		opposition = Type.GetType("NewEnemy");
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();

		foreach(PlayerBehaviour behaviour in this.animator.GetBehaviours<PlayerBehaviour>()) {
			behaviour.SetVar(this.GetComponent<Player>());
		}
		
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 100;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination= 10;
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
		}
	}
	
	// Update is called once per frame
	protected override void Update () {

		if (break_free)
			break_free = false;

		if (stunned) {
			tapped = true;

			float now = Time.time;
			float since = now - last_pressed;
			last_pressed = now;

			if(since > 0) {
				float motion = 1.0f / since;
				motion *= motion;
				mash_value++;
				if(mash_value > mash_threshold) break_free = true;
			}
		}

		if(isDead) return;

		if(UI != null && UI.onState){
			ItemCooldowns();
			UI.hpBar.max = stats.maxHealth;
			UI.greyBar.max = stats.maxHealth;
			UI.greyBar.current = stats.health+greyDamage;
			UI.hpBar.current = stats.health;
		}

		freeAnim = !stunned && !knockedback && !animationLock && !this.animator.GetBool ("IsInAttackAnimation");
		actable = freeAnim;

		this.animator.SetBool("Actable", this.actable);
		
		ActionCommands ();
		MoveCommands ();
		AnimationUpdate ();
	}

	public bool mashed_out(){
		return break_free;
	}

	
	//-------------------------------//
	// Player Command Implementation //
	//-------------------------------//
	
	protected override void ActionCommands() {
		// Invokes an action/animation
		if (stunned) {
			if(Input.GetKeyDown(controls.attack) || Input.GetButtonDown(controls.joyAttack)) {
			tapped = true;

			float now = Time.time;
			float since = now - last_pressed;
			last_pressed = now;

			if(since > 0) {
				float motion = 1.0f / since;
				motion *= motion;
				mash_value++;
				if(mash_value > mash_threshold) break_free = true;
			}
		}
		}

		if (actable && !this.animator.GetBool ("Attack")) {
			if(Input.GetKeyDown(controls.attack) || Input.GetButtonDown(controls.joyAttack)) {
				if(currDoor!=null){
					currDoor.GetComponent<Door>().toggleOpen();
					currDoor = null;
				} else {
					this.animator.SetBool("Charging", true);
					this.animator.SetBool("Attack", true);
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

	// Might separate commands into a protected function and just have a movement function
	protected virtual void MoveCommands() {
		Vector3 newMoveDir = Vector3.zero;
		Vector3 camAngle = Camera.main.transform.eulerAngles;
		
		if (actable) {
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
			
			if (!this.lockRotation && newMoveDir != Vector3.zero) {
				newMoveDir.y = 0.0f;
				facing = newMoveDir;
			}
			
			this.rb.velocity = newMoveDir.normalized * stats.speed * stats.spdManip.speedPercent;
		} else {
			// Right now this stops momentum when performing an action
			// If we trash the rigidbody later, we won't need this
			this.rb.velocity = Vector3.zero;
		}

		//Debug.Log(this.rb.velocity.magnitude);
	}
	
	// Constant animation updates (Main loop for characters movement/actions, sets important parameters in the animator)
	protected override void AnimationUpdate() {
		MovementAnimation();
	}
	
	//-------------------------------------------//


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
		
		//particle effects
		if(sparks == null){
			sparks = Instantiate(Resources.Load("Sparks"), transform.position, Quaternion.identity) as GameObject;
			Material particleMat = Resources.Load("Materials/" + this.gameObject.name + "Sparks", typeof(Material)) as Material;
			sparks.GetComponent<ParticleRenderer>().material = particleMat;
			Destroy (sparks, 1);
		}
		
		hitConfirm = new Knockback(gameObject.transform.position-atkPosition.position,(float) dmgTaken/stats.maxHealth*25.0f);
		BDS.addBuffDebuff(hitConfirm,gameObject,.5f);
		
	}
	
	public override void damage(int dmgTaken) {

		if (invincible || stats.isDead) return;
		
		stats.health -= greyTest(dmgTaken);
		if (stats.health <= 0) base.die();
		
		if(splatter == null) return;
		splatCore theSplat = ((GameObject)Instantiate (splatter, transform.position-new Vector3(0,.5f,0), Quaternion.identity)).GetComponent<splatCore>();
		theSplat.adjuster = (float) ((stats.maxHealth-stats.health)/stats.maxHealth);
		Destroy (theSplat, 2);
		
		//particle effects
		if(sparks == null){
			sparks = Instantiate(Resources.Load("Sparks"), transform.position, Quaternion.identity) as GameObject;
			Material particleMat = Resources.Load("Materials/" + this.gameObject.name + "Sparks", typeof(Material)) as Material;
			sparks.GetComponent<ParticleRenderer>().material = particleMat;
			Destroy (sparks, 1);
		}
	}
	
	public override void die() {
		base.die ();
		stats.health = 0;
		this.greyDamage = 0;
		if(UI!=null) UI.hpBar.current = 0;

		// Renderer[] rs = GetComponentsInChildren<Renderer>();
		this.isDead = true;
		this.animator.SetInteger("Killed", (int) UnityEngine.Random.Range(1.1f, 2.9f));	
	}
	
	public virtual void Death() {
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}
		//this will go to the end screen when all the players in the party are dead.
		if (!checkPartyAlive ()) {
			//THIS WILL NEED TO USE THE GSmanager HERE
			Application.LoadLevel("TitleScreen2");
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
			this.animator.SetInteger ("Killed", 0);
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

	//---------------------------------------
	//checkAlive()
	//---------------------------------------
	//
	//Checks to see how many players are still alive in the scene, if there is more than 1 then it will return true
	//If all the players are dead then it will return false. 
	//THIS FUNCTION NEEDS PLAYERS TO BE TAGGED AS PLAYERS
	//---------------------------------------
	private bool checkPartyAlive ()
	{
		int numbPlayersAlive = 0;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		int numbPlayers = players.Length;
		Character character;

		for(int i = 0; i < players.Length; i++)
		{
			//this gets the character component of the player
			character = players[i].GetComponent<Character>();
			
			//checks if the character is dead or not
			if(character != null && !character.isDead)
			{
				//if not add to number of players alive.
				numbPlayersAlive++;
			}

		}
		print("pdThere are " + numbPlayersAlive + " Players alive.");

		if (numbPlayersAlive == 0 && numbPlayers != 0) {
			return false;
		} else {
			return true;
		}
	}

}