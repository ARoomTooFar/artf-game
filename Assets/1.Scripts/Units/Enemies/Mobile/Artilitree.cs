using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Artilitree: MobileEnemy {
	
	// protected Stealth stealth;
	protected TargetCircle curTCircle;
	
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=2;
		stats.luck=0;
		
		this.minAtkRadius = 40.0f;
		this.maxAtkRadius = 100.0f;
	}
	
	protected override void initStates() {
		base.initStates();
		
		// Initialize all states
		State aquiringTarget = new State("aquiringTarget");
		
		
		// Add all the states to the state machine
		sM.states.Add (aquiringTarget.id, aquiringTarget);
		
		
		// Initialize all transitions
		Transition tAquiringTarget = new Transition(aquiringTarget);
		
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tAquiringTarget.targetState.id, tAquiringTarget);
		
		
		// Set conditions for the transitions
		tAquiringTarget.addCondition(this.isAquiringTarget);
		
		
		// Set actions for the states
		aquiringTarget.addAction (this.AquireTarget);
		
		
		// Sets exit actions for states
		// charging.addExitAction (this.doneCharge);
		
		
		// Set the transitions for the states
		// aquiringTarget.addTransition();
		
		
		// Adds transitions to old States
		this.addTransitionToExisting("attack", tAquiringTarget);
		
		// Adds old transitiont to new States
		this.addTransitionToNew("attackAnimation", aquiringTarget);
				
		// Remove Transition from old states
		this.removeTransitionFromExisting("attack", "attackAnimation");
	}
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	protected virtual bool isAquiringTarget() {
		if (this.animator.GetBool ("Charging")) {
			if (this.gear.weapon.GetComponent<Artillery>().curCircle != null) {
				this.curTCircle = this.gear.weapon.GetComponent<Artillery>().curCircle;
				return true;
			}
		}
		return false;
	}
	
	protected override bool isInAtkAnimation() {
		if ((this.attacking || this.animSteHash == this.atkHashChgSwing || this.animSteHash == this.atkHashCharge) && !this.animator.GetBool("Charging")) {
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			return true;
		}
		return false;
	}
	
	//----------------------//
	
	//-------------------//
	// Actions Functions //
	//-------------------//
	
	protected override void Attack() {
		if (this.actable && !attacking && this.curTCircle == null){
			this.getFacingTowardsTarget();
			this.transform.localRotation = Quaternion.LookRotation(facing);
			this.gear.weapon.initAttack();
			animator.SetBool("Charging", true);
		}
	}
	
	protected virtual void AquireTarget() {
		if (this.target != null && this.curTCircle != null && Vector3.Distance(this.curTCircle.transform.position, this.target.transform.position) > 1.5f) {
			this.curTCircle.moveCircle(this.target.transform.position - this.curTCircle.transform.position);
			this.getFacingTowardsTarget(); // Swap to facing target circle in future
			this.transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			this.animator.SetBool("Charging", false);
		}
		
	}
	
	//-------------------//
}
