using UnityEngine;
using System.Collections;

public class FoliantHive : StationaryEnemy {

	public int maxSpawn;
	public float spawnCD;

	protected int currentSpawn;
	protected float nextUse;
	protected ShootFodderBall shootFodderBall;
	protected TargetCircle curTCircle;

	protected override void Awake () {
		base.Awake ();
	}

	protected override void Start (){
		base.Start ();
		currentSpawn = 0;
		nextUse = Time.time + spawnCD;

		this.shootFodderBall = this.inventory.items[inventory.selected].GetComponent<ShootFodderBall>();
		if (this.shootFodderBall == null) print("Foliant Hive has no spawner detected");

	}

	protected override void Update() {
		base.Update ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 25;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 5;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}

	protected override void initStates() {
		//base.initStates ();

		// Initialize all states
		State rest = new State ("rest");
		State spawn = new State ("spawn");

		sM.states.Add (rest.id, rest);
		sM.states.Add (spawn.id, spawn);

		sM.initState = rest;		
		
		// Initialize all transitions
		Transition tRest = new Transition (rest);
		Transition tSpawn = new Transition (spawn);
		
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tRest.targetState.id, tRest);
		sM.transitions.Add (tSpawn.targetState.id, tSpawn);
		
		
		// Set conditions for the transitions
		tSpawn.addCondition (this.canSpawn);
		tRest.addCondition (this.canRest);
		
		
		// Set actions for the states
		spawn.addAction (this.doSpawn);
		rest.addAction (this.doRest);

		//Set transitions
		spawn.addTransition (tRest);
		rest.addTransition (tSpawn);

	}

	//----------------------//
	// Transition Functions //
	//----------------------//

	//Checks if it is allowed to spawn more fodder

	protected virtual bool canSpawn(){

		if (currentSpawn < maxSpawn && spawnOffCD ()){
			return true;
		}

		return false;
	}

	//Add in code if we want Hive to return to rest state
	protected virtual bool canRest(){
		return false;
	}

	protected virtual bool spawnOffCD(){
		if (Time.time >= nextUse) {
			return true;
		}
		return false;
	}

	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//
	
	
	protected virtual void doSpawn() {
		if (canSpawn()) {
			nextUse = Time.time + spawnCD;
			currentSpawn++;
			shootFodderBall.useItem();
		}
	}

	protected virtual void doRest(){
	}
	
	//-------------------//
}

