using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CackleBranch: RangedEnemy {
	
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

		
		this.minAtkRadius = 8.0f;
		this.maxAtkRadius = 40.0f;
	}


	protected override void SetTierData(int tier) {
		base.SetTierData (tier);

		monsterLoot.initializeLoot("CackleBranch", tier);
	}

	public override void SetInitValues(int health, int strength, int coordination, int armor, float speed) {
		base.SetInitValues(health, strength, coordination, armor, speed);
		this.gun = this.gear.weapon.GetComponent<CacklebranchPistol>();
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