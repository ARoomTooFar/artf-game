using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Artilitree: MobileEnemy {

	protected int rootTime;
	public float timing;
	protected bool rooted, rooting, unrooting;
	protected CoroutineController rootController;
	protected Artillery artillery;
	protected TargetCircle curTCircle;
	
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
		this.rooted = false;
		this.rooting = false;
		this.unrooting = false;

		this.artillery = this.gear.weapon.GetComponent<Artillery>();
		if (this.artillery == null) print("Artilitree has no artillery equipped");
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 200;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=2;
		stats.luck=0;

		this.rootTime = 3;
		this.timing = rootTime;
		this.minAtkRadius = 40.0f;
		this.maxAtkRadius = 250.0f;
	}
	
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

		// Artilitree States
		State inRange = new State ("inRange");
		State outRange = new State ("outRange");
		State rootingSelf = new State ("rootingSelf");
		State rootedSelf = new State ("rootedSelf");
		State unRootSelf = new State ("unRootSelf");
		State unRootedSelf = new State ("unRootedSelf");
		State aquiringTarget = new State("aquiringTarget");
		
		// Add all the states to the state machine
		sM.states.Add (rest.id, rest);
		sM.states.Add (approach.id, approach);
		sM.states.Add (attack.id, attack);
		sM.states.Add (atkAnimation.id, atkAnimation);
		sM.states.Add (search.id, search);
		sM.states.Add (retreat.id, retreat);
		sM.states.Add (spacing.id, spacing);
		sM.states.Add (far.id, far);

		sM.states.Add (inRange.id, inRange);
		sM.states.Add (outRange.id, outRange);
		sM.states.Add (rootingSelf.id, rootingSelf);
		sM.states.Add (rootedSelf.id, rootedSelf);
		sM.states.Add (unRootSelf.id, unRootSelf);
		sM.states.Add (unRootedSelf.id, unRootedSelf);
		sM.states.Add (aquiringTarget.id, aquiringTarget);
		
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

		Transition tInRange = new Transition (inRange);
		Transition tOutRange = new Transition (outRange);
		Transition tRootingSelf = new Transition (rootingSelf);
		Transition tRootedSelf = new Transition (rootedSelf);
		Transition tUnRootSelf = new Transition (unRootSelf);
		Transition tUnRootedSelf = new Transition (unRootedSelf);
		Transition tAquiringTarget = new Transition (aquiringTarget);
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tRest.targetState.id, tRest);
		sM.transitions.Add (tApproach.targetState.id, tApproach);
		sM.transitions.Add (tAttack.targetState.id, tAttack);
		sM.transitions.Add (tAtkAnimation.targetState.id, tAtkAnimation);
		sM.transitions.Add (tSearch.targetState.id, tSearch);
		sM.transitions.Add (tRetreat.targetState.id, tRetreat);
		sM.transitions.Add (tSpace.targetState.id, tSpace);
		sM.transitions.Add (tFar.targetState.id, tFar);

		sM.transitions.Add (tInRange.targetState.id, tInRange);
		sM.transitions.Add (tOutRange.targetState.id, tOutRange);
		sM.transitions.Add (tRootingSelf.targetState.id, tRootingSelf);
		sM.transitions.Add (tRootedSelf.targetState.id, tRootedSelf);
		sM.transitions.Add (tUnRootSelf.targetState.id, tUnRootSelf);
		sM.transitions.Add (tAquiringTarget.targetState.id, tAquiringTarget);
		
		// Set conditions for the transitions
		tApproach.addCondition(isApproaching);
		tRest.addCondition (isResting);
		tAttack.addCondition (isAttacking);
		tAtkAnimation.addCondition (isInAtkAnimation);
		tSearch.addCondition (isSearching);
		tRetreat.addCondition (isRetreating);
		tSpace.addCondition (isCreatingSpace);
		tFar.addCondition (isFar);

		tInRange.addCondition (this.isInRange);
		tOutRange.addCondition (this.isOutOfRange);
		tRootingSelf.addCondition (this.isRooting);
		tRootedSelf.addCondition (this.isRooted);
		tUnRootSelf.addCondition (this.isUnRooting);
		tUnRootedSelf.addCondition (this.isUnRooted);
		tAquiringTarget.addCondition(this.isAquiringTarget);
		
		
		// Set actions for the states
		rest.addAction (Rest);
		approach.addAction (Approach);
		attack.addAction (Attack);
		atkAnimation.addAction (AtkAnimation);
		search.addAction (Search);
		retreat.addAction (Retreat);
		spacing.addAction (Spacing);
		far.addAction (Far);

		inRange.addAction (this.InRange);
		outRange.addAction (this.OutRange);
		rootingSelf.addAction (this.RootingSelf);
		rootedSelf.addAction (this.RootedSelf);
		unRootSelf.addAction (this.UnRootSelf);
		unRootedSelf.addAction (this.AfterUpRoot);
		aquiringTarget.addAction (this.AquireTarget);

		// Sets enter actions for states
		attack.addEnterAction (this.EnterAttack);
		rootingSelf.addEnterAction (this.EnterRooting);
		unRootSelf.addEnterAction (this.EnterUnRooting);

		// Sets exit actions for states
		search.addExitAction (StopSearch);
		rootingSelf.addExitAction (this.ExitRooting);
		unRootSelf.addExitAction (this.ExitUnRooting);
		
		
		// Set the transitions for the states
		rest.addTransition (tApproach);
		approach.addTransition (tInRange);
		approach.addTransition (tSpace);
		approach.addTransition (tSearch);
		inRange.addTransition (tRootingSelf);

		rootingSelf.addTransition (tOutRange);
		rootingSelf.addTransition (tRootedSelf);

		rootedSelf.addTransition (tOutRange);
		rootedSelf.addTransition (tAttack);

		attack.addTransition (tAquiringTarget);
		aquiringTarget.addTransition (tAtkAnimation);

		atkAnimation.addTransition (tRootedSelf);

		outRange.addTransition (tUnRootSelf);

		unRootSelf.addTransition (tInRange);
		unRootSelf.addTransition (tUnRootedSelf);

		unRootedSelf.addTransition (tApproach);
		unRootedSelf.addTransition (tSearch);
		unRootedSelf.addTransition (tSpace);
		unRootedSelf.addTransition (tInRange);
		

		// search.addTransition (tAttack);
		search.addTransition (tApproach);
		search.addTransition (tRetreat);
		search.addTransition (tSpace);
		retreat.addTransition (tRest);
		retreat.addTransition (tApproach);
		spacing.addTransition (tFar);
		far.addTransition (tInRange);
		far.addTransition (tApproach);
		far.addTransition (tSearch);
	}
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	protected virtual bool isAquiringTarget() {
		if (this.animator.GetBool ("Charging")) {
			if (this.artillery.curCircle != null) {
				this.curTCircle = this.gear.weapon.GetComponent<Artillery>().curCircle;
				Vector3 direction = (this.target.transform.position - this.transform.position);
				direction.y = 0.0f;
				direction = direction.normalized * 4.0f;
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

	protected override bool isAttacking() {
		if (this.target != null && !this.isInAtkAnimation() && this.curTCircle == null) {
			float distance = this.distanceToPlayer(this.target);
			//float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
			//&& this.canSeePlayer(this.target)
			return distance < this.maxAtkRadius && distance > this.minAtkRadius;
		}
		return false;
	}

	protected virtual bool isInRange() {
		if (this.target != null && !this.isInAtkAnimation()) {
			float distance = this.distanceToPlayer(this.target);
			return distance < this.maxAtkRadius && distance > this.minAtkRadius;
		}
		return false;
	}

	// Also considers if they're too close
	protected virtual bool isOutOfRange() {
		if (this.target != null && !this.isInAtkAnimation()) {
			float distance = this.distanceToPlayer(this.target);
			return distance > this.maxAtkRadius || distance < this.minAtkRadius;
		}
		return true;
	}

	protected virtual bool isRooting() {
		return this.rooting;
	}

	protected virtual bool isRooted() {
		return this.rooted;
	}

	protected virtual bool isUnRooting () {
		return !this.rooting;
	}

	protected virtual bool isUnRooted() {
		return !this.rooted && !this.unrooting;
	}
	
	//----------------------//
	
	//-------------------//
	// Actions Functions //
	//-------------------//

	protected virtual void InRange() {
		this.rooting = true;
		this.unrooting = false;
	}

	protected virtual void OutRange() {
		this.rooting = false;
		this.unrooting = true;
	}

	// Rooting Actions
	protected virtual void EnterRooting() {
		if (!this.rooted) this.timing = this.rootTime;
		this.StartCoroutineEx(rootSelf(this.timing), out this.rootController);
		this.rb.isKinematic = true;
	}

	protected virtual void RootingSelf() {
		this.getFacingTowardsTarget();
		this.transform.localRotation = Quaternion.LookRotation(facing);
		this.rb.velocity = Vector3.zero;
	}

	protected virtual void ExitRooting() {
		this.rootController.Stop ();
		this.rb.isKinematic = false;
	}


	protected virtual void RootedSelf() {
		this.getFacingTowardsTarget();
		this.transform.localRotation = Quaternion.LookRotation(facing);
	}


	// Unrooting action
	protected virtual void EnterUnRooting() {
		if (this.rooted) this.timing = this.rootTime;
		this.StartCoroutineEx(unRoot(this.timing), out this.rootController);
		this.rb.isKinematic = true;
	}

	protected virtual void UnRootSelf() {
		this.getFacingTowardsTarget();
		this.transform.localRotation = Quaternion.LookRotation(facing);
		this.rb.velocity = Vector3.zero;
	}

	protected virtual void ExitUnRooting() {
		this.rootController.Stop ();
		this.rb.isKinematic = false;
	}

	// Just a state to go to after unrooting to transition to other states
	protected virtual void AfterUpRoot() {
	}

	protected override void EnterAttack() {
		if (this.actable && !attacking){
			this.getFacingTowardsTarget();
			this.transform.localRotation = Quaternion.LookRotation(facing);
			this.gear.weapon.initAttack();
			animator.SetBool("Charging", true);
		}
	}

	protected override void Attack() {
	}
	
	protected virtual void AquireTarget() {
		if (Vector3.Distance(this.transform.position, this.curTCircle.transform.position) > 4.0f && (this.target != null && this.curTCircle != null && Vector3.Distance(this.curTCircle.transform.position, this.target.transform.position) > 1.5f)) {
			this.curTCircle.moveCircle(this.target.transform.position - this.curTCircle.transform.position);
			this.getFacingTowardsTarget(); // Swap to facing target circle in future
			this.transform.localRotation = Quaternion.LookRotation(facing);
		} else {
			this.animator.SetBool("Charging", false);
		}
		
	}
	
	//-------------------//
	
	//------------//
	// Coroutines //
	//------------//
	
	protected virtual IEnumerator rootSelf(float time) {
		this.timing = 0.0f;
		while (this.timing < time) {
			this.timing += Time.deltaTime;
			yield return null;
		}
		// this.timing = this.rootTime;
		this.rooted = true;
	}
	
	protected virtual IEnumerator unRoot(float time) {
		this.timing = 0.0f;
		while (this.timing < time) {
			this.timing += Time.deltaTime;
			yield return null;
		}
		// this.timing = this.rootTime;
		this.unrooting = false;
		this.rooted = false;
	}
	
	//------------//
}
