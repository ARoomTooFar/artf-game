// Parent scripts for enemy units

using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character {

	//Aggro Variables
	public float dmgTimer = 0f;
	public bool aggro = false;

	public int tier;
	public AoETargetting aRange;
	protected StateMachine sM;
	
	protected float fov = 150f;
	protected float lineofsight = 15f;
	public float maxAtkRadius, minAtkRadius;
	
	// Variables for use in player detection
	protected bool alerted = false;
	public GameObject target;
	protected Vector3? lastSeenPosition = null;
	protected AggroTable aggroT;
	protected bool targetchanged;
	public EnemyHealthBar hpBar;


	protected int layerMask = 1 << 9;

	protected float aggroTimer = 7.0f;

	/*
	void OnEnable()
	{
		Player.OnDeath += playerDied;
	}
	
	
	void OnDisable()
	{
		Player.OnDeath -= playerDied;
	}*/

	protected override void Awake() {
		base.Awake();
		opposition = Type.GetType ("Player");
		hpBar = gameObject.GetComponentInChildren<EnemyHealthBar>();
		if(hpBar != null){
			hpBar.health = stats.health/stats.maxHealth;
		}
		facing = Vector3.back;
		targetchanged = false;

		// aRange.opposition = this.opposition;
		aRange.affectPlayers = true;
		
		//State machine initialization
		if (testing) {
			sM = new StateMachine ();
			initStates ();
			sM.Start ();
		}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();

		aggroT = new AggroTable();
		
	}

	// Update is called once per frame
	protected override void Update () {
		if (!stats.isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);

			animSteInfo = animator.GetCurrentAnimatorStateInfo (0);
			animSteHash = animSteInfo.fullPathHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd;
			
			
			//If aggro'd, will chase, and if not attacked for 5 seconds, will deaggro
			//If we want a deaggro function, change the if statement to require broken line of sight to target
			/*if (aggro == true) {
				fAggro ();
			}*/

			if (isGrounded) {
				MovementAnimation ();
				sM.Update ();
			} else {
				falling ();
			}

			// if (target != null)
			//	target = aggroT.getTarget ();
			target = aggroT.GetTopAggro();
			
		}
	}


	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=4;
		setAnimHash ();
	}
	
	protected virtual void initStates() {
	}

	protected virtual void SetStates() {
	}

	// For subclasses that want to add transitions to existing states
	protected void addTransitionToExisting(string stateId, Transition t) {
		State tempState; // For getting States that already exist within the State Machine

		if (this.sM.states.TryGetValue(stateId, out tempState)) {
			tempState.addTransition(t);
		}
	}

	// For subclasses that want to transition to old states from new states
	protected void addTransitionToNew(string stateId, State s) {
		Transition tempTransition; // For getting States that already exist within the State Machine

		if (this.sM.transitions.TryGetValue(stateId, out tempTransition)) {
			s.addTransition(tempTransition);
		}
	}
	
	protected void removeTransitionFromExisting(string stateId, string transitionStateId) {
		Transition tempTransition;
		State tempState;
		
		if (this.sM.transitions.TryGetValue(transitionStateId, out tempTransition)) {
			if (this.sM.states.TryGetValue(stateId, out tempState)) {
				tempState.removeTransition(tempTransition);
			}
		}
	}

	//-----------------------//
	// Calculation Functions //
	//-----------------------//
	
	protected float distanceToPlayer(GameObject p) {
		if (p == null) return 0.0f;
		Vector3 distance = p.transform.position - this.transform.position;
		return distance.sqrMagnitude;
	}

	protected virtual bool canSeePlayer(GameObject p) {
		if (p == null) return false;
	
		// Check angle of forward direction vector against the vector of enemy position relative to player position
		Vector3 direction = p.transform.position - transform.position;
		float angle = Vector3.Angle(direction, this.facing);
		
		if (angle < fov) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, lineofsight, layerMask)) {

				return false;

			}else{
				Player addable = p.GetComponent<Player>();
				if(!addable.invis){
					// aggroT.add(p,1);
					aggroT.AddAggro(p, 1);
					lastSeenPosition = p.transform.position;
					alerted = true;
				}else{
					return false;
				}
				return true;
	
			}
		}
		
		return false;
	}
	
	// Will change units facing to be towards their target. If new facing is zero it doesn't changes
	protected virtual void getFacingTowardsTarget() {
		Vector3 newFacing = Vector3.zero;

		if (this.target != null) {
			newFacing = this.target.transform.position - this.transform.position;
			newFacing.y = 0.0f;
			if (newFacing != Vector3.zero) this.facing = newFacing.normalized;
		}
	}
	
	//----------------------//


	//-------------------------------//
	// Character Inherited Functions //
	//-------------------------------//

	public override void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		if (this.invincible) return;
		this.damage(dmgTaken, atkPosition);
		aggroT.AddAggro(source, dmgTaken);
		isHit = true;
	}

	public override void damage(int dmgTaken, Transform atkPosition) {
		base.damage(dmgTaken, atkPosition);
		if(hpBar != null){
			Debug.Log((float)stats.health/stats.maxHealth);
			hpBar.health = (float)stats.health/stats.maxHealth;
		}
		if (aggro == false) {
			aggro = true;
			dmgTimer = 0f;
		}

		isHit = true;

	}
	
	public override void damage(int dmgTaken) {
		//aggro is on and timer reset if attacked
		if (aggro == false) {
			aggro = true;
			dmgTimer = 0f;
		}

		base.damage(dmgTaken);
		if(hpBar != null){
			Debug.Log((float)stats.health/stats.maxHealth);
			hpBar.health =(float) stats.health/stats.maxHealth;
		}

		isHit = true;
	}

	public override void die() {
		base.die ();
		stats.health = 0;
		//UI.hpBar.current = 0;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position-new Vector3(0,6,0), transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}
	}

	//-------------------------------//

	//-----------------//
	// Aggro Functions //
	//-----------------//

	public virtual void fAggro(){
		if (dmgTimer < 5f)
		{
			dmgTimer += Time.deltaTime;
		}
		else if (dmgTimer >= 5f)
		{
			resetAggro ();
			dmgTimer = 0f;
		}
	}
	
	public virtual void resetAggro(){
		dmgTimer = 0f;
		aggro = false;
		target = null;
	}

	/*
	public virtual void playerDied(GameObject dead){
		if (aggroT != null) {
			aggroT.RemoveUnit(dead);
		}
	}*/
	
	public virtual void playerVanished(GameObject dead){
		if (aggroT != null) {
			aggroT.RemoveUnit(dead);
			target = null;
		}
	}
	
	public virtual void taunted(GameObject taunter){
		if (aggroT != null){
			aggroT.AddAggro(taunter,aggroT.GetAggro()*2);
		}
	}
	
	//---------------//
}