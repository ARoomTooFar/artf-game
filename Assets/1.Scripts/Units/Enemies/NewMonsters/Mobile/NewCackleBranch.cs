using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewCackleBranch: NewRangedEnemy {
	
	protected Roll roll;

	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();

		this.roll = this.inventory.items[inventory.selected].GetComponent<Roll>();
		if (this.roll == null) Debug.LogWarning ("CackleBranch does not have roll equipped");

		foreach(CackleRoll behaviour in this.animator.GetBehaviours<CackleRoll>()) {
			behaviour.roll = this.roll;
		}
		
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 30;
		stats.health = stats.maxHealth;
		stats.armor = 1;
		stats.strength = 10;
		stats.coordination=5;
		stats.speed=7;
		stats.luck=0;
		
		this.minAtkRadius = 8.0f;
		this.maxAtkRadius = 40.0f;
	}
	

	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//
	

	//-------------------//
	
	
	//------------//
	// Coroutines //
	//------------//

	
	
	//------------//
	
	
	//------------------//
	// Helper Functions //
	//------------------//
	
	
	//------------------//
}