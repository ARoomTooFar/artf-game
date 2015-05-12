using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NewCackleBranch: NewRangedEnemy {
	
	protected Roll roll;
	protected CacklebranchPistol gun;

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

		this.gun = this.gear.weapon.GetComponent<CacklebranchPistol>();
		
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 1;
		stats.strength = 0;
		stats.coordination= 10;
		stats.speed=7;
		
		this.minAtkRadius = 8.0f;
		this.maxAtkRadius = 40.0f;
	}


	public override void SetTierData(int tier) {
		tier = 0;
		base.SetTierData (tier);

		monsterLoot.initializeLoot("CackleBranch", tier);
	}

	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//

	public virtual void Shoot(int count) {
		this.StartCoroutine(this.gun.Shoot(count));
	}

	public override void die() {
		this.isDead = true;
		animator.SetTrigger("Died");
		
	}
	
	public virtual void Death() {
		base.die ();
	}

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