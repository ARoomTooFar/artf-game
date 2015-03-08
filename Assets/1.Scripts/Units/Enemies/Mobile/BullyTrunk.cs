using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BullyTrunk: MobileEnemy {

	private BullCharge charge;

	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
		charge = this.inventory.items[inventory.selected].GetComponent<BullCharge>();
		if (charge == null) Debug.LogWarning ("BullyTrunk does not have charge equipped");
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
		stats.speed=9;
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


		// Set the transitions for the states
		charging.addTransition(tCharge);


		// Adds transitions to old States
		this.addTransitionToExisting("approach", tCharging);

		// Adds old transitiont to new States
		this.addTransitionToNew("approach", charge);
		this.addTransitionToNew("search", charge);
	}

	//----------------------//
	// Transition Functions //
	//----------------------//

	// Bully Trunk charging code
	protected virtual bool isTooFar () {
		if (this.target != null && Vector3.Distance(this.transform.position, this.target.transform.position) > this.maxAtkRadius && this.charge.curCoolDown <= 0) {
			this.inventory.keepItemActive = true;
			inventory.items[inventory.selected].useItem();
			return true;
		}
		return false;
	}

	protected virtual bool isWithinCharge () {
		if (this.target == null) {
			return true;
		} else {
			Vector3 tPos = this.target.transform.position;
			if (Vector3.Distance(this.transform.position, tPos) <= this.charge.curChgTime * 3 || Vector3.Distance(this.transform.position, tPos) >= 15 || this.charge.curChgTime >= this.charge.maxChgTime) {
				return true;
			}
			return false;
		}
	}

	//----------------------//

	//-------------------//
	// Actions Functions //
	//-------------------//

	protected virtual void chargingCharge () {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		this.GetComponent<Rigidbody>().velocity = (this.facing.normalized * stats.speed * stats.spdManip.speedPercent)/2;
		if (!this.canSeePlayer(this.target)) {
			this.target = null;
		}
	}

	protected virtual void doneCharge() {
		this.inventory.keepItemActive = false;
	}

	protected virtual void chargingIntoSucker () {
		if (this.target != null) {
			this.lastSeenPosition = this.target.transform.position;
		}
	}

	//-------------------//
}