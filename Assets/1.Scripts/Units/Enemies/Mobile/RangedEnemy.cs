// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RangedEnemy : MobileEnemy {

	//-------------------//
	// Primary Functions //
	//-------------------//
	
	// Get players, navmesh and all colliders
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();

		if (this.lastSeenPosition.HasValue) {
			float distance = Vector3.Distance(this.transform.position, this.lastSeenPosition.Value);
			this.animator.SetBool ("TooClose", distance < this.minAtkRadius );
			this.animator.SetBool ("FarEnough", distance > this.minAtkRadius + 2.5f);
		} else {
			this.animator.SetBool ("HasLastSeenPosition", false);
			this.animator.SetBool ("FarEnough", true);
			this.animator.SetBool ("TooClose", false);
		}
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		
		this.minAtkRadius = 5.0f;
		this.maxAtkRadius = 12.0f;
	}
	
	//----------------------//
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	
	//---------------------//
	
	
	//------------------//
	// Action Functions //
	//------------------//
	
	
	//------------------//
	
	
	
	//-----------------------------//
	// Coroutines for timing stuff //
	//-----------------------------//

	
	//-----------------------------//
	
	
	//-----------------------//
	// Calculation Functions //
	//-----------------------//
	
	//-----------------//
}
