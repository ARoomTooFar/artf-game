// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobileEnemy : NewEnemy {

	protected Vector3 resetpos;

	protected float tempTimer;

	//-------------------//
	// Primary Functions //
	//-------------------//
	
	// Get players, navmesh and all colliders
	protected override void Awake () {
		base.Awake ();
		resetpos = transform.position;
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
		stats.speed=9;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;
	}
	
	// Initializes states, transitions and actions
	protected override void initStates() {
		
		// Initialize all states
		State rest = new State("rest");
		State approach = new State("approach");
		State attack = new State ("attack");
		State atkAnimation = new State ("attackAnimation");
		State search = new State ("search");
		State retreat = new State ("retreat");
		State spacing = new State ("space");
		State far = new State ("far");

	//	State blankState = new State("blank"); // For testing purposes
		
		
		// Add all the states to the state machine
		sM.states.Add (rest.id, rest);
		sM.states.Add (approach.id, approach);
		sM.states.Add (attack.id, attack);
		sM.states.Add (atkAnimation.id, atkAnimation);
		sM.states.Add (search.id, search);
		sM.states.Add (retreat.id, retreat);
		sM.states.Add (spacing.id, spacing);
		sM.states.Add (far.id, far);
		
		
		// Set initial state for the State Machine of this unit
		sM.initState = rest;
		
		
		// Initialize all transitions
		Transition tRest = new Transition(rest);
		Transition tApproach = new Transition (approach);
		Transition tAttack = new Transition (attack);
		Transition tAtkAnimation = new Transition(atkAnimation);
		Transition tSearch = new Transition(search);
		Transition tRetreat = new Transition (retreat);
		Transition tSpace = new Transition (spacing);
		Transition tFar = new Transition (far);
		
		
		// Set conditions for the transitions
		tApproach.addCondition(isApproaching, this);
		tRest.addCondition (isResting, this);
		tAttack.addCondition (isAttacking, this);
		tAtkAnimation.addCondition (isInAtkAnimation, this);
		tSearch.addCondition (isSearching, this);
		tRetreat.addCondition (isRetreating, this);
		tSpace.addCondition (isCreatingSpace, this);
		tFar.addCondition (isFar, this);

		
		// Set actions for the states
		rest.addAction (Rest, this);
		approach.addAction (Approach, this);
		attack.addAction (Attack, this);
		atkAnimation.addAction (AtkAnimation, this);
		search.addAction (Search, this);
		retreat.addAction (Retreat, this);
		spacing.addAction (Spacing, this);
		far.addAction (Far, this);
		
		
		// Set the transitions for the states
		rest.addTransition (tApproach);
		approach.addTransition (tAttack);
		approach.addTransition (tSearch);
		approach.addTransition (tSpace);
		attack.addTransition (tAtkAnimation);
		atkAnimation.addTransition (tApproach);
		atkAnimation.addTransition (tAttack);
		atkAnimation.addTransition (tSearch);
		atkAnimation.addTransition (tSpace);
		search.addTransition (tApproach);
		search.addTransition (tAttack);
		search.addTransition (tRetreat);
		search.addTransition (tSpace);
		retreat.addTransition (tRest);
		retreat.addTransition (tApproach);
		spacing.addTransition (tFar);
		far.addTransition (tApproach);
		far.addTransition (tAttack);
		far.addTransition (tSearch);
	}
	
	//----------------------//
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	protected virtual bool isResting(Character a) {
		return this.lastSeenPosition == null && !this.alerted;
	}
	
	protected virtual bool isApproaching(Character a) {
		// If we don't have a target currently and aren't alerted, automatically assign anyone in range that we can see as our target
		if (this.target == null) {// && !this.alerted) {
			if (aRange.inRange.Count > 0) {
				foreach(Character tars in aRange.inRange) {
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
		
		if (distance >= this.maxAtkRadius && this.canSeePlayer (this.target) && !isInAtkAnimation(a)) {
			// agent.alerted = true;
			return true;
		}
		return false;
	}
	
	protected virtual bool isAttacking(Character a) {
		if (this.target != null) {
			float distance = this.distanceToPlayer(this.target);
			return distance < this.maxAtkRadius && distance >= this.minAtkRadius && this.canSeePlayer(this.target);
		}
		return false;
	}
	
	protected virtual bool isInAtkAnimation(Character a) {
		if (this.attacking || this.animSteHash == this.atkHashChgSwing || this.animSteHash == this.atkHashCharge) {
			this.rigidbody.velocity = Vector3.zero;
			return true;
		}
		return false;
	}
	
	protected virtual bool isSearching(Character a) {
		return this.lastSeenPosition.HasValue && !(this.canSeePlayer (this.target) && this.alerted) && !this.isInAtkAnimation(a);
	}
	
	
	protected virtual bool isRetreating(Character a) {
		return this.target == null && !this.alerted;
	}

	protected virtual bool isCreatingSpace(Character a) {
		if (this.target != null && !this.isInAtkAnimation(a)) {
			float distance = this.distanceToPlayer(this.target);
			return distance < this.minAtkRadius && this.canSeePlayer(this.target);
		}
		return false;
	}

	protected virtual bool isFar (Character a) {
		if (this.target != null) {
			float distance = this.distanceToPlayer(this.target);
			return distance > this.minAtkRadius;
		}
		return false;
	}
	
	//---------------------//
	
	
	//------------------//
	// Action Functions //
	//------------------//
	
	protected virtual void Rest(Character a) {
		// this.resetpos = this.transform.position;
		

		if (!this.isApproaching(this) && tempTimer > 0.0f) {
			tempTimer -= Time.deltaTime;
		} else {
			tempTimer = 0.5f;

			this.resetpos = this.transform.position;
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
		}

	}
	
	protected virtual void Approach(Character a) {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
	}
	
	protected virtual void Attack(Character a) {
		if (this.actable && !attacking){
			this.gear.weapon.initAttack();
		}
	}
	
	// We can have some logic here, but it's mostly so our unit is still during and attack animation
	protected virtual void AtkAnimation(Character a) {
	}
	
	protected virtual void Search(Character a) {
		target = null;
		if (this.lastSeenPosition.HasValue) {
			this.facing = this.lastSeenPosition.Value - this.transform.position;
			this.facing.y = 0.0f;
			StartCoroutine(searchForEnemy(this.lastSeenPosition.Value));
			this.lastSeenPosition = null;
		}
	}
	
	//Improve retreat AI
	protected virtual void Retreat(Character a) {
		// agent.nav.destination = agent.retreatPos;
		this.facing = this.resetpos - this.transform.position;
		StartCoroutine(moveToPosition(this.resetpos));
	}

	protected virtual void Spacing(Character a) {
		this.facing = (this.target.transform.position - this.transform.position) * -1;
		this.facing.y = 0.0f;
		this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
	}

	protected virtual void Far (Character a) {
		this.facing = this.facing * -1;
	}
	
	//------------------//
	
	
	
	//-----------------------------//
	// Coroutines for timing stuff //
	//-----------------------------//
	
	protected IEnumerator moveToPosition(Vector3 position) {
		while (!this.isApproaching(this) && this.distanceToVector3(position) > 0.5f) {
			this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return null;
		}
	}

	protected IEnumerator randomSearch() {
		float resetTimer = aggroTimer;
		while(!this.isApproaching(this) && resetTimer > 0.0f) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return new WaitForSeconds (0.5f);
			resetTimer -= 0.5f;
		}
	}
	
	protected virtual IEnumerator searchForEnemy(Vector3 lsp) {
		yield return StartCoroutine(moveToPosition(lsp));
		
		float resetTimer = aggroTimer;

		yield return StartCoroutine(randomSearch());

		/*
		while(!this.isApproaching(this) && resetTimer > 0.0f) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return new WaitForSeconds (0.5f);
			resetTimer -= 0.5f;
		}*/
		if (this.target == null) alerted = false;
	}
	
	//-----------------------------//
	
	
	//-----------------------//
	// Calculation Functions //
	//-----------------------//
	
	protected float distanceToVector3(Vector3 position) {
		Vector3 distance = position - this.transform.position;
		return distance.sqrMagnitude;
	}
	
	//-----------------//
}
