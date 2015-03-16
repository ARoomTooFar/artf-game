﻿// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobileEnemy : Enemy {

	protected Vector3 resetpos;
	protected Vector3 targetDir;

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
		//Debug.Log (stats.health);
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


		// Add all the transitions to the state machine
		sM.transitions.Add (tRest.targetState.id, tRest);
		sM.transitions.Add (tApproach.targetState.id, tApproach);
		sM.transitions.Add (tAttack.targetState.id, tAttack);
		sM.transitions.Add (tAtkAnimation.targetState.id, tAtkAnimation);
		sM.transitions.Add (tSearch.targetState.id, tSearch);
		sM.transitions.Add (tRetreat.targetState.id, tRetreat);
		sM.transitions.Add (tSpace.targetState.id, tSpace);
		sM.transitions.Add (tFar.targetState.id, tFar);
		
		// Set conditions for the transitions
		tApproach.addCondition(isApproaching);
		tRest.addCondition (isResting);
		tAttack.addCondition (isAttacking);
		tAtkAnimation.addCondition (isInAtkAnimation);
		tSearch.addCondition (isSearching);
		tRetreat.addCondition (isRetreating);
		tSpace.addCondition (isCreatingSpace);
		tFar.addCondition (isFar);

		
		// Set actions for the states
		rest.addAction (Rest);
		approach.addAction (Approach);
		attack.addAction (Attack);
		atkAnimation.addAction (AtkAnimation);
		search.addAction (Search);
		retreat.addAction (Retreat);
		spacing.addAction (Spacing);
		far.addAction (Far);


		// Sets exit actions for states
		search.addExitAction (StopSearch);
		
		
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
		search.addTransition (tAttack);
		search.addTransition (tApproach);
		search.addTransition (tRetreat);
		search.addTransition (tSpace);
		retreat.addTransition (tRest);
		retreat.addTransition (tApproach);
		// spacing.addTransition (tApproach);
		spacing.addTransition (tFar);
		far.addTransition (tApproach);
		far.addTransition (tAttack);
		far.addTransition (tSearch);
	}
	
	//----------------------//
	
	//----------------------//
	// Transition Functions //
	//----------------------//

	protected virtual bool isResting() {
		return this.lastSeenPosition == null && !this.alerted;
	}
	
	protected virtual bool isApproaching() {
		// If we don't have a target currently and aren't alerted, automatically assign anyone in range that we can see as our target
		if (this.actable) {
			if (this.target == null) {// && !this.alerted) {
				if (aRange.inRange.Count > 0) {
					foreach(Character tars in aRange.inRange) {
						if (this.canSeePlayer(tars.gameObject) && !tars.isDead) {
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
			if (this.canSeePlayer (this.target) && !isInAtkAnimation()) {
				// agent.alerted = true;
				return true;
			}
		}
		return false;
	}
	
	protected virtual bool isAttacking() {
		if (this.target != null && !this.isInAtkAnimation()) {
			float distance = this.distanceToPlayer(this.target);
			//&& this.canSeePlayer(this.target)
			return distance < this.maxAtkRadius && distance >= this.minAtkRadius;
		}
		return false;
	}
	
	protected virtual bool isInAtkAnimation() {
		if (this.attacking || this.animSteHash == this.atkHashChgSwing || this.animSteHash == this.atkHashCharge) {
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			return true;
		}
		return false;
	}

	protected virtual bool isSearching() {
		if (this.target == null || (this.lastSeenPosition.HasValue && !(this.canSeePlayer (this.target) && this.alerted) && !this.isInAtkAnimation()) && this.actable) {
			return true;
		}
		return false;
	}
	
	
	protected virtual bool isRetreating() {
		return this.target == null && !this.alerted;
	}

	protected virtual bool isCreatingSpace() {
		if (this.target != null && !this.isInAtkAnimation()) {
			float distance = this.distanceToPlayer(this.target);
			return distance < this.minAtkRadius && this.canSeePlayer(this.target);
		}
		return false;
	}

	protected virtual bool isFar () {
		if (this.target != null && this.actable) {
			float distance = this.distanceToPlayer(this.target);
			return distance > this.minAtkRadius;
		}
		if (this.target == null) {
			return true;
		}
		return false;
	}
	
	//---------------------//
	
	
	//------------------//
	// Action Functions //
	//------------------//
	
	protected virtual void Rest() {
		// this.resetpos = this.transform.position;
		this.StopSearch ();
		if (!this.isApproaching() && tempTimer > 0.0f) {
			tempTimer -= Time.deltaTime;
		} else {
			tempTimer = 0.5f;

			this.resetpos = this.transform.position;
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
		}

	}
	
	protected virtual void Approach() {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
	}

	protected virtual void Attack() {
		if (this.actable && !attacking){
			this.facing = this.target.transform.position - this.transform.position;
			this.facing.y = 0.0f;
			transform.localRotation = Quaternion.LookRotation(facing);
			this.gear.weapon.initAttack();
		}
	}
	
	// We can have some logic here, but it's mostly so our unit is still during and attack animation
	protected virtual void AtkAnimation() {
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		this.isApproaching();
	}
	
	protected virtual void Search() {
		target = null;
		if (this.lastSeenPosition.HasValue) {
			this.facing = this.lastSeenPosition.Value - this.transform.position;
			this.facing.y = 0.0f;
			StartCoroutine("searchForEnemy", this.lastSeenPosition.Value);
			this.lastSeenPosition = null;
		}
	}

	protected virtual void StopSearch() {
		this.StopCoroutine("searchForEnemy");
		this.StopCoroutine("moveToPosition");
		this.StopCoroutine("moveToExpectedArea");
		this.StopCoroutine("randomSearch");
	}
	
	//Improve retreat AI
	protected virtual void Retreat() {
		// agent.nav.destination = agent.retreatPos;
		this.facing = this.resetpos - this.transform.position;
		StartCoroutine(moveToPosition(this.resetpos));
	}

	protected virtual void Spacing() {
		this.facing = (this.lastSeenPosition.Value - this.transform.position) * -1;
		this.facing.y = 0.0f;
		this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
	}

	protected virtual void Far () {
		this.facing = this.facing * -1;
	}
	
	//------------------//
	
	
	
	//-----------------------------//
	// Coroutines for timing stuff //
	//-----------------------------//
	
	protected IEnumerator moveToPosition(Vector3 position) {
		while (!this.isApproaching() && this.distanceToVector3(position) > 0.1f && !this.isInAtkAnimation() && this.target == null) {
			this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return null;
		}
	}

	protected IEnumerator moveToExpectedArea() {
		this.facing = this.targetDir;
		float moveToTime = 0.5f;
		while (!this.isApproaching() && this.distanceToVector3(this.targetDir) > 0.1f && !this.isInAtkAnimation() && this.target == null && moveToTime > 0.0f) {
			this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			moveToTime -= Time.deltaTime;
			yield return null;
		}
	}

	protected IEnumerator randomSearch() {
		float resetTimer = aggroTimer;
		while(!this.isApproaching() && resetTimer > 0.0f && !this.isInAtkAnimation() && this.target == null) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.GetComponent<Rigidbody>().velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return new WaitForSeconds (0.5f);
			resetTimer -= 0.5f;
		}
	}
	
	protected virtual IEnumerator searchForEnemy(Vector3 lsp) {
		yield return StartCoroutine("moveToPosition", lsp);

		yield return StartCoroutine ("moveToExpectedArea");

		float resetTimer = aggroTimer;

		yield return StartCoroutine("randomSearch");

		if (this.target == null) alerted = false;
	}
	
	//-----------------------------//
	
	
	//-----------------------//
	// Calculation Functions //
	//-----------------------//

	protected override bool canSeePlayer(GameObject p) {
		Vector3 direction = p.transform.position - transform.position;
		float angle = Vector3.Angle(direction, this.facing);
		

		if (angle < fov) {

			/*
			RaycastHit hit;

			// if (GetComponent<Rigidbody>().SweepTest(direction, out hit, this.distanceToVector3(p.GetComponent<Collider>().transform.position))) {
			if (GetComponent<Rigidbody>().SweepTest(direction, out hit, Vector3.Distance(this.transform.position, p.GetComponent<Collider>().transform.position))) {
				if (hit.collider.gameObject == p) {
					aggroT.add(p,1);
					if (this.lastSeenPosition != null) {
						this.targetDir = p.transform.position - this.lastSeenPosition.Value;
						this.targetDir.Normalize();
					}
					this.lastSeenPosition = p.transform.position;
					this.alerted = true;
					return true;
				}
			}*/


			RaycastHit[] hits;

			hits = GetComponent<Rigidbody>().SweepTestAll(direction, Vector3.Distance(this.transform.position, p.transform.position) + 2);
			
			for (int i = 0; i < hits.Length; ++i) {
				// print(hits[i].transform.name);
				if (hits[i].collider.gameObject == p) {
					aggroT.add(p,1);
					if (this.lastSeenPosition != null) {
						this.targetDir = this.lastSeenPosition.Value - p.transform.position;
						this.targetDir.Normalize();
					}
					this.lastSeenPosition = p.transform.position;
					this.alerted = true;
					return true;
				}
				if (hits[i].collider.tag == "Wall") return false;
			}
		}
		
		return false;


	}

	protected float distanceToVector3(Vector3 position) {
		Vector3 distance = position - this.transform.position;
		return distance.sqrMagnitude;
	}
	
	//-----------------//
}