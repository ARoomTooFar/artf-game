using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FoliantFodder: MobileEnemy {
	
	protected Roll roll;

	public FoliantHive hive;
	public bool hiveMind;

	protected override void Awake () {
		hiveMind = false;
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
		roll = this.inventory.items[inventory.selected].GetComponent<Roll>();
		if (roll == null) Debug.LogWarning ("FoliantFodder does not have lunge equipped");
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
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}
	
	protected override void initStates() {
		base.initStates();
		
		// Initialize all states
		State lunge = new State ("lunge");
		
		
		// Add all the states to the state machine
		sM.states.Add (lunge.id, lunge);
		
		
		// Initialize all transitions
		Transition tLunge = new Transition(lunge);
		
		
		// Add all the transitions to the state machine
		sM.transitions.Add (tLunge.targetState.id, tLunge);
		
		
		// Set conditions for the transitions
		tLunge.addCondition (this.isWithinLunge);
		
		
		// Set actions for the states
		lunge.addAction (this.doLunge);
		
		
		// Adds transitions to old States
		this.addTransitionToExisting("approach", tLunge);
		
		// Adds old transitions to new States
		this.addTransitionToNew("approach", lunge);
		this.addTransitionToNew("attack", lunge);
		this.addTransitionToNew("search", lunge);
	}
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	// Foliant Fodder lunge code
	
	protected virtual bool isWithinLunge () {
		if (this.target == null) {
			return true;
		} else {
			Vector3 tPos = this.target.transform.position;
			if (Vector3.Distance(this.transform.position, tPos) >= 10 &&  this.roll.curCoolDown <= 0) {
				return true;
			}
			return false;
		}
	}
	
	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//

	
	protected virtual void doLunge() {

		this.roll.useItem();
	
	}
	
	//-------------------//
	
	
	//------------------//
	// Public Functions //
	//------------------//
	
	public virtual void setHive(FoliantHive parent)
	{
		hiveMind = true;
		hive = parent;
		base.aggroT = hive.hiveMindAggro;

	}

	public virtual void hiveDied()
	{
		hiveMind = false;
		hive = null;
		base.aggroT = new AggroTable ();
	}

	//------------------//

	//--------------------------------//
	//Mobile Enemy Inherited Functions//
	//--------------------------------//
	
	public override void die() {

		if (hiveMind) {
			hive.removeFodder(this);
		}

		base.die ();
	}
	
	//--------------------------------//
	
	

}