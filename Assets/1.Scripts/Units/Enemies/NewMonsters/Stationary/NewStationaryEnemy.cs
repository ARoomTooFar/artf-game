using UnityEngine;
using System.Collections;

public class NewStationaryEnemy : NewEnemy  {

	//-------------------//
	// Primary Functions //
	//-------------------//
	
	// Get players, navmesh and all colliders
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
		this.rb.isKinematic = true;
	}
	
	protected override void Update() {
		base.Update ();
		
		if (this.target != null && this.target.GetComponent<Player>() != null) {
			this.targetDir = this.target.GetComponent<Player>().facing;
		}
		
		if (!this.lastSeenPosition.HasValue) {
			this.animator.SetBool ("HasLastSeenPosition", false);
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
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 4.0f;
	}
	
	//----------------------//
}
