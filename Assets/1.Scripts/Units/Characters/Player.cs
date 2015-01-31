// Character class
// Base class that our heroes will inherit from

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Controls {
	//First 7 are Keys, last 2 are joystick axis
	public string up, down, left, right, attack, secItem, cycItem, hori, vert;
	//0 for Joystick off, 1 for Joystick on and no keys
	public int joyUsed;
}

[RequireComponent(typeof(Rigidbody))]
public class Player : Character, IMoveable, IStunable<float>, IForcible<float> {
	public bool inGrey;
	public int testDmg;
	public int greyDamage;
	public bool testable;

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
		testable = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(stats.health <= 0){
			isDead = true;
		} else {
			isDead = false;
		}
		if(!isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
			
			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.nameHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd ;
			
			if (isGrounded) {
				actionCommands ();
				moveCommands ();
			} else {
				falling();
			}
			
			animationUpdate ();
		}
	}

	//-----------------------------------//
	// Movement interface implementation //
	//-----------------------------------//
	
	// Might separate commands into a protected function and just have a movement function
	public virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;
		
		if (actable || animSteHash == atkHashCharge) {//gear.weapon.stats.curChgAtkTime > 0) { // Better Check here
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

	//-------------------------------//
	// Stun Interface Implementation //
	//-------------------------------//
	 
	public virtual void stun(float stunDuration) {
		print ("Stunned for " + stunDuration + " seconds");
	}

	//-------------------------------//

	//--------------------------------//
	// Force Interface Implementation //
	//--------------------------------//

	// The duration are essentially stun, expand on these later
	public virtual void pull(float pullDuration) {
		stun(pullDuration);
	}

	public virtual void push(float pushDuration) {
		stun(pushDuration);
	}

	//--------------------------------//

	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Character striker) {
		if (!invincible) {
			dmgTaken = Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(facing, transform.position, striker.transform.position)), 1, 100000);
		
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			
			if (stats.health <= 0) {
				die();
			}
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible) {
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);

			if (stats.health <= 0) {
				die();
			}
		}
	}

	public override void die() {
		base.die();
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
				return stats.health/20 + tempDmg;
			}else{
				inGrey = true;
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+(damage - stats.maxHealth/20));
				//print("True!NGAT:"+stats.maxHealth/20);
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				return stats.maxHealth/20;
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
			testable = true;
			yield return 0;
		}
	}
	private IEnumerator RegenWait(){
		yield return new WaitForSeconds(1);
		if(inGrey && !stats.isDead){
			// print("Healed Grey and True");
			greyDamage--;
			if(greyDamage > 0){
				StartCoroutine("RegenWait");
			}
		}
		yield return 0;
	}

	//----------------------------------//
}