using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bushman : MobileEnemy {


	private class BMFrenzyGrowth : StatsMultiplier {
		public float basedmg, dmgGrowth;
		public float basedef, defGrowth;
		public float basespd, spdGrowth;

		// variable growth
		public StatsMultiplier FrenzyGrowth(float dmgUp, float dmgRedUp, float spdUp) {
			StatsMultiplier nextLv = new StatsMultiplier ();
			nextLv.dmgAmp = basedmg + dmgUp * dmgGrowth;
			nextLv.dmgRed = basedef + dmgRedUp * defGrowth;
			nextLv.speed = basespd + spdUp * spdGrowth;
			return nextLv;
		}

		// constant growth
		public StatsMultiplier CFrenzyGrowth(int currentStage){
			StatsMultiplier nextLv = new StatsMultiplier ();
			nextLv.dmgAmp = basedmg + currentStage * dmgGrowth;
			nextLv.dmgRed = basedef + currentStage * defGrowth;
			nextLv.speed = basespd + currentStage * spdGrowth;
			return nextLv;
		}
	}
	
	private bool inFrenzy;
	private PowerLevels powlvs;
	private Frenzy frenzy;
	private float health;
	protected Sprint sprint;
	protected BullCharge charge;
	protected Roll lungeAttack;

	
	protected override void initStates() {
		base.initStates();

		// Initialize all states
		State charging = new State("charging");
		State charge = new State ("charge");
		State lunge = new State ("lunge");
		State free_anim = new State ("free anim");
		
		
		// Add all the states to the state machine
		sM.states.Add (charging.id, charging);
		sM.states.Add (charge.id, charge);
		sM.states.Add (lunge.id, lunge);
		sM.states.Add (free_anim.id, free_anim);
		
		
		// Initialize all transitions
		Transition tCharging = new Transition(charging);
		Transition tCharge = new Transition(charge);
		Transition tLunge = new Transition (lunge);
		Transition tFreeAnim = new Transition (free_anim);
		
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tCharging.targetState.id, tCharging);
		sM.transitions.Add (tCharge.targetState.id, tCharge);
		sM.transitions.Add (tLunge.targetState.id, tLunge);
		sM.transitions.Add (tFreeAnim.targetState.id, tFreeAnim);
		
		
		// Set conditions for the transitions
		tCharging.addCondition(this.isTooFar);
		tCharge.addCondition (this.isWithinCharge);
		tLunge.addCondition (this.reAggro);
		tFreeAnim.addCondition (this.freestate);
		
		
		// Set actions for the states
		charging.addAction (this.chargingCharge);
		charge.addAction (this.chargingIntoSucker);
		lunge.addAction (this.switchTarget);
		free_anim.addAction (this.freeAction);
		
		
		// Sets exit actions for states
		charging.addExitAction (this.doneCharge);
		charge.addExitAction (this.chargeEnd);
		
		
		// Set the transitions for the states
		charging.addTransition(tCharge);
		
		
		// Adds transitions to old States
		this.addTransitionToExisting("approach", tCharging);
		this.addTransitionToExisting("approach", tLunge);
		this.addTransitionToExisting("search", tLunge);
		this.addTransitionToExisting("attackAnimation", tLunge);
		this.addTransitionToExisting("space", tLunge);


		// Adds old transitiont to new States
		this.addTransitionToNew("free anim", charge);
		this.addTransitionToNew("free anim", lunge);
		this.addTransitionToNew ("approach", free_anim);
		this.addTransitionToNew ("search", free_anim);
		this.addTransitionToNew ("attack", free_anim);
		this.addTransitionToNew ("space", free_anim);
		this.addTransitionToNew ("retreat", free_anim);

	}
	
	protected override void Awake () {
		base.Awake ();
		inFrenzy = false;
		health = stats.health;


	}

	protected override void Start() {
		base.Start ();
		charge = this.inventory.items[inventory.selected].GetComponent<BullCharge>();
		lungeAttack = this.inventory.items[++inventory.selected].GetComponent<Roll>();
		if (charge == null) Debug.LogWarning ("Bushmen does not have charge equipped");
		setFrenzy ();
	}
	
	protected override void Update() {
		if(health > stats.health){
			health = stats.health;
			powlvs.addRage(Mathf.CeilToInt((float)(stats.maxHealth - stats.health)/stats.maxHealth * 150));
		}
		powlvs.Update();
		base.Update ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=6;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;
	}

	protected void setFrenzy() {
		powlvs = new PowerLevels (this);

		BMFrenzyGrowth bmf = new BMFrenzyGrowth ();
		bmf.basedmg = 0f; bmf.basedef = 0f; bmf.basespd = 0f;
		bmf.dmgGrowth = 0.1f; bmf.defGrowth = -0.1f; bmf.spdGrowth = 0.1f;

		powlvs.addStage(bmf.FrenzyGrowth(0f, 0f, 0f), 0);
		powlvs.addStage(bmf.FrenzyGrowth(2f, 1f, 1f), 10);
		powlvs.addStage(bmf.FrenzyGrowth(3f, 2.5f, 2f), 20);
		powlvs.addStage(bmf.FrenzyGrowth(3.6f, 3f, 2.5f), 25);
		powlvs.addStage(bmf.FrenzyGrowth(5f, 3.5f, 4f), 36);
		powlvs.addStage(bmf.FrenzyGrowth(7.5f, 8f, 6f), 45);
		powlvs.addStage(bmf.FrenzyGrowth(10f, 8f, 12f), 60);
	}

	// all floats represent percent increase
	protected StatsMultiplier FrenzyGrowth(float dmgUp, float dmgRedUp, float spdUp, float currentGrowth) {
		StatsMultiplier nextLv = new StatsMultiplier ();
		nextLv.dmgAmp = dmgUp * currentGrowth;
		nextLv.dmgRed = dmgRedUp * currentGrowth;
		nextLv.speed = spdUp * currentGrowth;
		return nextLv;
	}

	//----------------------//
	// Transition Functions //
	//----------------------//

	protected virtual bool isTooFar () {
		if (charge == null) Debug.LogWarning ("bushmannnnnnn does not have charge equipped");
		if (this.target != null && Vector3.Distance(this.transform.position, this.target.transform.position) > this.maxAtkRadius && this.charge.curCoolDown <= 0) {
			this.inventory.keepItemActive = true;
			charge.useItem();
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

	protected bool reAggro(){
		return targetchanged;
	}

	protected bool freestate(){
		return this.freeAnim;
	}

	//-------------------//
	// Actions Functions //
	//-------------------//
	
	protected virtual void chargingCharge () {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		this.GetComponent<Rigidbody>().velocity = (this.facing.normalized * stats.speed * stats.spdManip.speedPercent);
		if (!this.canSeePlayer(this.target)) {
			this.target = null;
		}
	}

	protected void freeAction() {

	}
	

	protected virtual void doneCharge() {
		this.inventory.keepItemActive = false;
	}
	
	protected virtual void chargingIntoSucker () {
		if (this.target != null) {
			this.lastSeenPosition = this.target.transform.position;
			// this.lowerArms();
		}
	}
	
	protected virtual void chargeEnd() {
	}

	protected virtual void switchTarget() {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		lungeAttack.useItem ();
		targetchanged = false;
	}

	protected bool isFrenzy(){
		return inFrenzy;
	}
	

	protected bool isFrenzy(Character a){
		Bushman bushman = (Bushman) a;
		return bushman.isFrenzy ();
	}

	protected void Frenzy(){

	}
}
