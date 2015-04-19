using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewArtilitree: NewRangedEnemy {
	
	protected RootRing roots;
	protected Artillery artillery;

	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		
		this.artillery = this.gear.weapon.GetComponent<Artillery>();
		if (this.artillery == null) print("Artilitree has no artillery equipped");
		
		foreach(ArtilleryBehaviour behaviour in this.animator.GetBehaviours<ArtilleryBehaviour>()) {
			behaviour.SetVar(this.artillery);
		}
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
		stats.speed=7;
		stats.luck=0;
		
		this.minAtkRadius = 8.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	public override void SetTierData(int tier) {
		tier = 0;
		base.SetTierData(tier);
		
		if (tier > 4) {
			this.roots = this.inventory.items[inventory.selected].GetComponent<RootRing>();
			if (this.roots == null) Debug.LogWarning ("Artilitree does not have root ring equipped");
		}
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