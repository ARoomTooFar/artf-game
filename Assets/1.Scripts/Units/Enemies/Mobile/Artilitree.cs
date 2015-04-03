using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Artilitree: MobileEnemy {
	
	// protected Stealth stealth;
	
	protected bool rooted, rooting;
	protected TargetCircle curTCircle;
	
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
		this.rooted = false;
		this.rooting = false;
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
		State rootSelf = new State("rootSelf");
		
		
		// Add all the states to the state machine
		sM.states.Add (aquiringTarget.id, aquiringTarget);
		sM.states.Add (rootSelf.id, rootSelf);
		
		
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
				this.rooted = true;
				this.curTCircle = this.gear.weapon.GetComponent<Artillery>().curCircle;
				Vector3 direction = (this.target.transform.position - this.transform.position);
				direction.y = 0.0f;
				direction = direction.normalized * 3.0f;
				this.curTCircle.transform.position = new Vector3(this.curTCircle.transform.position.x + direction.x, this.curTCircle.transform.position.y, this.curTCircle.transform.position.z + direction.z);
				return true;
			}
		}
		return false;
	}
	
	protected override bool isInAtkAnimation() {
		if ((this.attacking || this.animSteHash == this.atkHashChgSwing || this.animSteHash == this.atkHashCharge) && !this.animator.GetBool("Charging")) {
			this.rb.velocity = Vector3.zero;
			return true;
		}
		return false;
	}
	
	protected virtual bool isRooted() {
		return this.rooted;
	}
	
	//----------------------//
	
	//-------------------//
	// Actions Functions //
	//-------------------//
	
	protected virtual void RootSelf() {
	}
	
	protected override void Attack() {
		if (this.actable && !attacking && this.curTCircle == null){
			this.getFacingTowardsTarget();
			this.transform.localRotation = Quaternion.LookRotation(facing);
			this.gear.weapon.initAttack();
			animator.SetBool("Charging", true);
		}
	}
	
	protected virtual void AquireTarget() {
		if (Vector3.Distance(this.transform.position, this.curTCircle.transform.position) > 3.0f && (this.target != null && this.curTCircle != null && Vector3.Distance(this.curTCircle.transform.position, this.target.transform.position) > 1.5f)) {
			this.curTCircle.moveCircle(this.target.transform.position - this.curTCircle.transform.position);
			this.getFacingTowardsTarget(); // Swap to facing target circle in future
			this.transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			this.animator.SetBool("Charging", false);
		}
		
	}
	
	protected virtual void Rooted() {
		this.rb.velocity = Vector3.zero;
	}
	
	//-------------------//
	
	//------------//
	// Coroutines //
	//------------//
	
	protected virtual IEnumerator rootSelf(float time) {
		this.rooting = true;
		while (time > 0.0f) {
			time -= Time.deltaTime;
			yield return null;
		}
		this.rooted = true;
		this.rooting = false;
	}
	
	protected virtual IEnumerator unRoot(float time) {
		while (time > 0.0f) {
			time -= Time.deltaTime;
			yield return null;
		}
		this.rooted = false;
	}
	
	//------------//
}
