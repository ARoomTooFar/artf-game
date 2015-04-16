// Enemies that can only stay in one place

using UnityEngine;
using System.Collections;

public class StationaryEnemy : Enemy {

	//-------------------//
	// Primary Functions //
	//-------------------//

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
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=4;
		stats.luck=0;
		setAnimHash ();

		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}

	// Initializes states, transitions and actions
	protected override void initStates() {
		// Initialize all states
		State rest = new State("rest");
		State approach = new State("approach");
		State attack = new State ("attack");
		State atkAnimation = new State ("attackAnimation");
		State search = new State ("search");
		
		
		// Add all the states to the state machine
		sM.states.Add (rest.id, rest);
		sM.states.Add (approach.id, approach);
		sM.states.Add (attack.id, attack);
		sM.states.Add (atkAnimation.id, atkAnimation);
		sM.states.Add (search.id, search);
		
		
		// Set initial state for the State Machine of this unit
		sM.initState = rest;
		
		
		// Initialize all transitions
		Transition tRest = new Transition(rest);
		Transition tApproach = new Transition(approach);
		Transition tAttack = new Transition (attack);
		Transition tAtkAnimation = new Transition(atkAnimation);
		Transition tSearch = new Transition (search);

		// Add all the transitions to the state machine
		sM.transitions.Add (tRest.targetState.id, tRest);
		sM.transitions.Add (tApproach.targetState.id, tApproach);
		sM.transitions.Add (tAttack.targetState.id, tAttack);
		sM.transitions.Add (tAtkAnimation.targetState.id, tAtkAnimation);
		sM.transitions.Add (tSearch.targetState.id, tSearch);
		
		
		// Set conditions for the transitions
		tRest.addCondition (isResting);
		tApproach.addCondition (isApproaching);
		tAttack.addCondition (isAttacking);
		tAtkAnimation.addCondition (isInAtkAnimation);
		tSearch.addCondition (isSearching);
		
		
		// Set actions for the states
		rest.addAction (Rest);
		approach.addAction (Approach);
		attack.addAction (Attack);
		atkAnimation.addAction (AtkAnimation);
		search.addAction (Search);
		
		
		// Set the transitions for the states
		rest.addTransition (tApproach);
		approach.addTransition (tAttack);
		approach.addTransition (tSearch);
		attack.addTransition (tAtkAnimation);
		atkAnimation.addTransition (tAttack);
		atkAnimation.addTransition (tApproach);
		atkAnimation.addTransition (tSearch);
		search.addTransition (tAttack);
		search.addTransition (tApproach);
		search.addTransition (tRest);
	}

	//------------------//

	//----------------------//
	// Transition Functions //
	//----------------------//

	protected virtual bool isResting() {
		return (this.lastSeenPosition == null && !this.alerted);
	}
	
	protected virtual bool isApproaching() {
		// If we don't have a target currently and aren't alerted, automatically assign anyone in range that we can see as our target
		if (this.target == null) {
			if (aRange.unitsInRange.Count > 0) {
				foreach(Character tars in aRange.unitsInRange) {
					if (this.canSeePlayer(tars.gameObject)) {
						this.alerted = true;
						target = tars.gameObject;
						break;
					}
				}
				
				if (target == null) {
					return false;
				}
			} else {
				return false;
			}
		}
		
		float distance = this.distanceToPlayer(this.target);
		
		if (distance >= this.maxAtkRadius && this.canSeePlayer (this.target) && !isInAtkAnimation()) {
			// agent.alerted = true;
			return true;
		}
		return false;
	}

	protected virtual bool isAttacking() {
		if (this.target != null) {
			float distance = this.distanceToPlayer(this.target);
			return distance < this.maxAtkRadius && distance >= this.minAtkRadius;
		}
		return false;
	}

	protected virtual bool isInAtkAnimation() {
		return this.attacking || this.animSteHash == this.atkHashChgSwing || this.animSteHash == this.atkHashCharge;
	}

	protected virtual bool isSearching() {
		return this.lastSeenPosition.HasValue && !(this.canSeePlayer (this.target) && this.alerted) && !this.isInAtkAnimation();
	}

	//----------------------//


	//------------------//
	// Action Functions //
	//------------------//

	protected virtual void Rest() {
		// Idle
	}

	protected virtual void Approach() {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
	}

	protected virtual void Attack() {
		if (this.actable && !attacking){
			this.gear.weapon.initAttack();
		}
	}

	// We can have some logic here, but it's mostly so our unit is still during and attack animation
	protected virtual void AtkAnimation() {
	}

	protected virtual void Search() {
		target = null;
		if (this.lastSeenPosition.HasValue) {
			this.facing = this.lastSeenPosition.Value - this.transform.position;
			this.facing.y = 0.0f;
			StartCoroutine(randomSearch());
			this.lastSeenPosition = null;
		}
	}

	//------------------//


	//-----------------------------//
	// Coroutines for timing stuff //
	//-----------------------------//

	protected virtual IEnumerator randomSearch() {
		float resetTimer = aggroTimer;
		while(!this.isApproaching() && resetTimer > 0.0f) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			yield return new WaitForSeconds (0.5f);
			resetTimer -= 0.5f;
		}
		if (this.target == null) alerted = false;
	}

	//---------------------------//

	//---------------------//
	// Inherited Functions //
	//---------------------//

	protected override void movementAnimation() {
		transform.localRotation = Quaternion.LookRotation(facing);
	}

	//----------
}
