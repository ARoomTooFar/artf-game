using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bushman : MobileEnemy {
	
	private bool inFrenzy;
	private float frenzy_counter;
	private float frenzy_growth;
	private PowerLevels powlvs;
	private Frenzy frenzy;
	private int tier;
	private float health;
	GameObject expDeath;
	protected Sprint sprint;
	protected BullCharge charge;
	protected Roll lungeAttack;

	
	protected override void initStates() {
		base.initStates();

		targetchanged = true;
		
		// Initialize all states
		State charging = new State("charging");
		State charge = new State ("charge");
//		State targetSwitch = new State ("switch target");
		State lunge = new State ("lunge");
		
		
		// Add all the states to the state machine
		sM.states.Add (charging.id, charging);
		sM.states.Add (charge.id, charge);
//		sM.states.Add (targetSwitch.id, targetSwitch);
		sM.states.Add (lunge.id, lunge);
		
		
		// Initialize all transitions
		Transition tCharging = new Transition(charging);
		Transition tCharge = new Transition(charge);
//		Transition tTargetSwitch = new Transition (targetSwitch);
		Transition tLunge = new Transition (lunge);
		
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tCharging.targetState.id, tCharging);
		sM.transitions.Add (tCharge.targetState.id, tCharge);
//		sM.transitions.Add (tTargetSwitch.targetState.id, tTargetSwitch);
		sM.transitions.Add (tLunge.targetState.id, tLunge);
		
		
		// Set conditions for the transitions
		tCharging.addCondition(this.isTooFar);
		tCharge.addCondition (this.isWithinCharge);
		tLunge.addCondition (this.reAggro);
		
		
		// Set actions for the states
		charging.addAction (this.chargingCharge);
		charge.addAction (this.chargingIntoSucker);
		lunge.addAction (this.switchTarget);
		
		
		// Sets exit actions for states
		charging.addExitAction (this.doneCharge);
		charge.addExitAction (this.chargeEnd);
		
		
		// Set the transitions for the states
		charging.addTransition(tCharge);
		
		
		// Adds transitions to old States
		this.addTransitionToExisting("approach", tCharging);
		this.addTransitionToExisting("approach", tLunge);
		this.addTransitionToExisting("search", tLunge);
		this.addTransitionToExisting("attack", tLunge);
		this.addTransitionToExisting("space", tLunge);


		// Adds old transitiont to new States
		this.addTransitionToNew("approach", charge);
		this.addTransitionToNew("search", charge);
		this.addTransitionToNew ("approach", lunge);
		this.addTransitionToNew ("search", lunge);
		this.addTransitionToNew ("attack", lunge);
		this.addTransitionToNew ("space", lunge);
		this.addTransitionToNew ("retreat", lunge);

	}
	
	protected override void Awake () {
		base.Awake ();
		inFrenzy = false;
		frenzy_counter = 0;
		frenzy_growth = 2;

		health = stats.health;


	}

	void setTier(int tier){
		this.tier = tier;
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
//		Debug.Log (stats.health);
		powlvs.Update();
		base.Update ();
	}

	/*
	public override void die() {
		Debug.Log("IsDead");
		base.die();
		stats.health = 0;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}
	} 
	 */



	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=6;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;
	}

	protected void setFrenzy() {
		powlvs = new PowerLevels (this);
		StatsMultiplier stage0 = new StatsMultiplier (); stage0.dmgAmp = 0f; stage0.dmgRed = 0f; stage0.speed = 0f;
		StatsMultiplier stage1 = new StatsMultiplier (); stage1.dmgAmp = 0.2f; stage1.dmgRed = -0.1f; stage1.speed = 0.1f;
		StatsMultiplier stage2 = new StatsMultiplier (); stage2.dmgAmp = 0.3f; stage2.dmgRed = -0.25f; stage2.speed = 0.2f;
		StatsMultiplier stage3 = new StatsMultiplier (); stage3.dmgAmp = 0.36f; stage3.dmgRed = -0.3f; stage3.speed = 0.25f;
		StatsMultiplier stage4 = new StatsMultiplier (); stage4.dmgAmp = 0.5f; stage4.dmgRed = -0.35f; stage4.speed = 0.4f;
		StatsMultiplier stage5 = new StatsMultiplier (); stage5.dmgAmp = 0.75f; stage5.dmgRed = -0.4f; stage5.speed = 0.6f;
		StatsMultiplier stage6 = new StatsMultiplier (); stage6.dmgAmp = 1.0f; stage6.dmgRed = -0.8f; stage6.speed = 1.2f;
		powlvs.addStage(stage0, 0);
		powlvs.addStage(stage1, 10);
		powlvs.addStage(stage2, 20);
		powlvs.addStage(stage3, 25);
		powlvs.addStage(stage4, 36);
		powlvs.addStage(stage5, 45);
		powlvs.addStage(stage6, 60);
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

	/*protected void LungeAttack(){

	}*/
	
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
//		this.blast.useItem();
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
