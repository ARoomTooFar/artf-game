// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewSwordEnemy : NewMobileEnemy {
	
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
		this.maxAtkRadius = 2.0f;
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
