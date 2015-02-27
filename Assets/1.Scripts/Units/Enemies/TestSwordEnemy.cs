using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TestSwordEnemy: Enemy {

	protected bool alerted = false;
	protected float fov = 150f;
	protected float lineofsight = 15f;

	public int waypointIndex = 0;

	protected float maxAtkRadius, minAtkRadius;

	// Waypoints for patrolling
	public List<Transform> patrolWP = new List<Transform>();

	protected Vector3? searchPosition = null;
	protected Vector3? lastSeenPosition = null;
	
	protected Vector3 resetpos;

	public AggroRange aRange;

	// Protected Variables
	protected NavMeshAgent nav;
	protected AggroTable aggroT;
	protected StateMachine sM;
	
	// Variables for use in player detection
	public GameObject target;
	private GameObject[] players;

	private float posTimer = 0f;
	public float aggroTimer = 10f;
	
	//-------------------//
	// Primary Functions //
	//-------------------//

	// Get players, navmesh and all colliders
	protected override void Awake ()
	{
		base.Awake ();
		facing = Vector3.back;
		aggroT = new AggroTable();
		nav = GetComponent<NavMeshAgent> ();

		resetpos = transform.position;
		patrolWP.Add (transform);

		aRange.opposition = this.opposition;

		//State machine initialization
		sM = new StateMachine ();
		initStates ();
		sM.Start ();
	}

	protected override void Update()
	{
		base.Update ();
		if (target != null) target = aggroT.getTarget ();
		if(target == null || !lastSeenPosition.HasValue){
			sM.Update();
			return;
		}

		/*
		bool iseeyou = canSeePlayer (target);
		if (target && lastSeenPosition.HasValue && !iseeyou) {
			posTimer += Time.deltaTime;
		} else if (target && iseeyou){
			posTimer = 0f;
		}*/
		
		//Speed updates from stats now, fix navigation to not overshoot like it does
		nav.speed = stats.speed;
		
		sM.Update ();
	}
	
	// Initializes states, transitions and actions
	protected virtual void initStates(){
		
		// Initialize all states
		State rest = new State("rest");
		State approach = new State("approach");
		State attack = new State ("attack");
		State atkAnimation = new State ("attackAnimation");
		State search = new State ("search");
		State retreat = new State ("retreat");


		// Add all the states to the state machine
		sM.states.Add (rest.id, rest);
		sM.states.Add (approach.id, approach);
		sM.states.Add (attack.id, attack);
		sM.states.Add (atkAnimation.id, atkAnimation);
		sM.states.Add (search.id, search);
		sM.states.Add (retreat.id, retreat);


		// Set initial state for the State Machine of this unit
		sM.initState = rest;


		// Initialize all transitions
		Transition tRest = new Transition(rest);
		Transition tApproach = new Transition (approach);
		Transition tAttack = new Transition (attack);
		Transition tAtkAnimation = new Transition(atkAnimation);
		Transition tSearch = new Transition(search);
		Transition tRetreat = new Transition (retreat);


		// Set conditions for the transitions
		tApproach.addCondition(isApproaching, this);
		tRest.addCondition (isResting, this);
		tAttack.addCondition (isAttacking, this);
		tAtkAnimation.addCondition (isInAtkAnimation, this);
		tSearch.addCondition (isSearching, this);
		tRetreat.addCondition (isRetreating, this);


		// Set actions for the states
		rest.addAction (Rest, this);
		approach.addAction (Approach, this);
		attack.addAction (Attack, this);
		atkAnimation.addAction (AtkAnimation, this);
		search.addAction (Search, this);
		retreat.addAction (Retreat, this);


		// Set the transitions for the states
		rest.addTransition (tApproach);
		approach.addTransition (tAttack);
		approach.addTransition (tSearch);
		attack.addTransition (tAtkAnimation);
		atkAnimation.addTransition (tApproach);
		atkAnimation.addTransition (tAttack);
		atkAnimation.addTransition (tSearch);
		search.addTransition (tApproach);
		search.addTransition (tAttack);
		search.addTransition (tRetreat);
		retreat.addTransition (tRest);
		retreat.addTransition (tApproach);
	}

	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;

		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;

		testDmg = 0;
	}
	
	//----------------------//

	//----------------------//
	// Transition Functions //
	//----------------------//

	protected virtual bool isResting(Character a) {
		TestSwordEnemy agent = (TestSwordEnemy)a;
		return (this.lastSeenPosition == null && !this.alerted);
	}

	protected virtual bool isApproaching(Character a) {
		nav.speed = stats.speed * stats.spdManip.speedPercent;

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
			return distance < this.maxAtkRadius && distance >= this.minAtkRadius;
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

	//---------------------//


	//------------------//
	// Action Functions //
	//------------------//

	protected virtual void Rest(Character a) {
		this.resetpos = this.transform.position;
		// Patrol (a);
	}

	protected virtual void Patrol(Character a) {
		nav.speed = (stats.speed * stats.spdManip.speedPercent)/2;
		this.animator.SetBool ("Moving", true);
		
		if (this.nav.remainingDistance < this.nav.stoppingDistance) {
			if(this.waypointIndex <= this.patrolWP.Count - 1){
				this.waypointIndex++;
			}
			else this.waypointIndex = 0;
			
		}
		
		// if(agent.patrolWP.Count > 0) agent.nav.destination = agent.patrolWP[waypointIndex].position;
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
			StartCoroutine(randomSearch(this.lastSeenPosition.Value));
			this.lastSeenPosition = null;
		}
	}

	//Improve retreat AI
	public void Retreat(Character a) {
		// agent.nav.destination = agent.retreatPos;
		this.facing = this.resetpos - this.transform.position;
		StartCoroutine(moveToPosition(this.resetpos));
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

	protected virtual IEnumerator randomSearch(Vector3 lsp) {
		yield return StartCoroutine(moveToPosition(lsp));
		
		float resetTimer = aggroTimer;
		while(!this.isApproaching(this) && resetTimer > 0.0f) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return new WaitForSeconds (0.5f);
			resetTimer -= 0.5f;
		}
		if (this.target == null) alerted = false;
	}
	
	//-----------------------------//


	//-----------------------//
	// Calculation Functions //
	//-----------------------//

	protected virtual bool canSeePlayer(GameObject p) {
		// Check angle of forward direction vector against the vector of enemy position relative to player position
		Vector3 direction = p.transform.position - transform.position;
		float angle = Vector3.Angle(direction, this.facing);
		
		if (angle < fov) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, lineofsight)) {
				
				if (hit.collider.gameObject == p) {
					aggroT.add(p,1);
					lastSeenPosition = p.transform.position;
					alerted = true;
					return true;
				}
			}
		}

		return false;
	}

	protected float distanceToPlayer(GameObject p) {
		if (p == null) return 0.0f;
		Vector3 distance = p.transform.position - this.transform.position;
		return distance.sqrMagnitude;
	}

	protected float distanceToVector3(Vector3 position) {
		Vector3 distance = position - this.transform.position;
		return distance.sqrMagnitude;
	}

	//-----------------//
}
