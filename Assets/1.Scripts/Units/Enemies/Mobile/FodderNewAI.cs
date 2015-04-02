using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FodderNewAI: MobileEnemy {

	protected ProtectBox pBox;

	//-------------------//
	// Primary Functions //
	//-------------------//
	
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
		pBox = new ProtectBox (this.transform.position);
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
		stats.speed=7;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;
	}

	protected override void initStates() {
		
		base.initStates();
		
		// Initialize all states
		State charging = new State("charging");
		State charge = new State ("charge");
		
		
		// Add all the states to the state machine
		sM.states.Add (charging.id, charging);
		sM.states.Add (charge.id, charge);
		
		
		// Initialize all transitions
		Transition tCharging = new Transition(charging);
		Transition tCharge = new Transition(charge);
		
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tCharging.targetState.id, tCharging);
		sM.transitions.Add (tCharge.targetState.id, tCharge);
		
		
		// Set conditions for the transitions
		tCharging.addCondition(this.isTooFar);
		tCharge.addCondition (this.isWithinCharge);
		
		
		// Set actions for the states
		charging.addAction (this.chargingCharge);
		charge.addAction (this.chargingIntoSucker);
		
		
		// Sets exit actions for states
		charging.addExitAction (this.doneCharge);
		charge.addExitAction (this.chargeEnd);
		
		
		// Set the transitions for the states
		charging.addTransition(tCharge);
		
		
		// Adds transitions to old States
		this.addTransitionToExisting("approach", tCharging);
		
		// Adds old transitiont to new States
		this.addTransitionToNew("approach", charge);
		this.addTransitionToNew("search", charge);
	}

	//----------------------//

	//----------------------//
	// Transition Functions //
	//----------------------//
	
	protected override bool isResting() {
		if(aRange.unitsInRange.Count == 0){
			return true;
		}else{
			return false;
		}
	}

	//----------------------//

	//------------------//
	// Action Functions //
	//------------------//

	protected override void Rest() {
		if (isGrounded) {
			this.resetpos.y = this.transform.position.y;
			if (this.distanceToVector3(this.resetpos) <= 0.01f){
				this.resetpos = pBox.getProtectV();
				this.resetpos.y = this.transform.position.y;
			}
			this.facing = this.resetpos - this.transform.position;
			this.facing.y = 0.0f;
			this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
		}
			


	}

}
