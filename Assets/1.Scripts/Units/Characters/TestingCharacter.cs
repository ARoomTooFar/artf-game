// Testing character is used

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class TestingCharacter : Character, IActionable, IFallable, IAttackable, IDamageable<int, Character>, ISlowable<float>, IStunable<float>, IForcible<float> {

	// Use this for initialization
	protected override void Start () {
		stats = new Stats(this.GetComponent<MonoBehaviour>());
		animator = GetComponent<Animator>();
		facing = Vector3.forward;
		isDead = false;
		freeAnim = true;
		setInitValues();
		gear.equipGear(this, opposition);
		inventory.equipItems(this);
		setAnimHash();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
	}
	
	// Gets hash code for animations (Faster than using string name when running)
	protected override void setAnimHash() {
		idleHash = Animator.StringToHash ("Base Layer.idle");
		runHash = Animator.StringToHash ("Base Layer.run");
		
		// atkHash = Animator.StringToHash ("Base Layer.attack");
		atkHashStart = Animator.StringToHash (weapTypeName + "." + weapTypeName + "Start");
		atkHashCharge = Animator.StringToHash (weapTypeName + "." + weapTypeName + "Charge");
		atkHashSwing = Animator.StringToHash (weapTypeName + "." + weapTypeName + "Swing");
		atkHashChgSwing = Animator.StringToHash (weapTypeName + "." + weapTypeName + "ChargedSwing");
		atkHashEnd = Animator.StringToHash (weapTypeName + "." + weapTypeName + "End");
	}
	
	protected override void FixedUpdate() {
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
			
			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.nameHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd ;
			
			if (isGrounded) {
				actionCommands ();
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
	
	// Animation helper functions
	protected override void attackAnimation() {
		// Should this also be in the weapons?
		
		/*
		if (gears.weapon.stats.curChgAtkTime > 0) {
			// Change once we get animations
			if(animSteInfo.normalizedTime < gears.weapon.stats.colStart) animator.speed = gears.weapon.stats.atkSpeed;
			else animator.speed = 0;
			if (rigidbody.velocity != Vector3.zero) transform.localRotation = Quaternion.LookRotation(facing);
		} else if (gears.weapon.stats.curChgAtkTime == -1) {
			animator.speed = gears.weapon.stats.atkSpeed;
		}*/
	}
	
	protected override void movementAnimation() {
		// animator.speed = 1; // Change animation speed back for other animations
		if (rigidbody.velocity != Vector3.zero) {
			animator.SetBool("Moving", true);
			transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			animator.SetBool("Moving", false);
		}
	}
	
	
	//----------------------------------//
	// Falling Interface Implementation //
	//----------------------------------//
	
	public override void falling() {
		// fake gravity
		// Animation make it so rigidbody gravity works oddly due to some gravity weight
		// Seems like Unity Pro is needed to change that, so unless we get it, this will suffice 
		rigidbody.velocity = new Vector3 (0.0f, -gravity, 0.0f);
	}
	
	//----------------------------------//
	
	
	//---------------------------------//
	// Attack Interface Implementation //
	//---------------------------------//
	
	// If using a basic attack, this will do checks (such as charging an attack)
	public override void attacks() {
		// gears.weapon.attack ();
	}
	
	//---------------------------------//
	
	
	
	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public override void damage(int dmgTaken, Character striker) {
		if (!invincible) {
			Mathf.Clamp(Mathf.RoundToInt(dmgTaken * stats.dmgManip.getDmgValue(facing, transform.position, striker.transform.position)), 1, 100000);
			
			stats.health -= dmgTaken;
			
			if (stats.health <= 0) {
				die();
			}
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible) {
			stats.health -= dmgTaken;
			
			if (stats.health <= 0) {
				die();
			}
		}
	}
	
	// Add logic to this in the future
	//     ie: Removing actions, player from camera etc
	public override void die() {
		stats.isDead = true;
	}
	
	//-------------------------------//
	
	//-------------------------------//
	// Slow Interface Implementation //
	//-------------------------------//
	//----------------------------------//
	public override void slow(float slowStrength) {
		stats.spdManip.setSpeedReduction(slowStrength);
	}
	
	public override void removeSlow(float slowStrength) {
		stats.spdManip.removeSpeedReduction(slowStrength);
	}
	
	public override void slowForDuration(float slowStrength, float slowDuration) {
		slow(slowStrength);
		StartCoroutine(buffTiming(slowStrength, slowDuration, removeSlow));
	}
	
	public override void speed(float speedStrength) {
		stats.spdManip.setSpeedAmplification(speedStrength);
	}
	
	public virtual void removeSpeed(float speedStrength) {
		stats.spdManip.removeSpeedAmplification(speedStrength);
	}
	
	public override void speedForDuration(float speedStrength, float speedDuration) {
		speed(speedStrength);
		StartCoroutine(buffTiming(speedStrength, speedDuration, removeSpeed));
	}
	
	//-------------------------------//
	
	
	//-------------------------------//
	// Stun Interface Implementation //
	//-------------------------------//
	
	public override void stun(float stunDuration) {
		print ("Stunned for " + stunDuration + " seconds");
	}
	
	//-------------------------------//
	
	
	//--------------------------------//
	// Force Interface Implementation //
	//--------------------------------//
	
	// The duration are essentially stun, expand on these later
	public override void pull(float pullDuration) {
		stun(pullDuration);
	}
	
	public override void push(float pushDuration) {
		stun(pushDuration);
	}
	
	//--------------------------------//
	
	
	//-----------------------------//
	// Timing Event Implementation //
	//-----------------------------//
	
	// Used for buffs that are duration based
	// Uses delegates to call function when over
	// Will make virtual when neccessary
	protected IEnumerator buffTiming(float strValue, float duration, BuffDelegate bd) {
		while (duration > 0) {
			duration -= Time.deltaTime;
			yield return null;
		}
		bd(strValue);
	}
}