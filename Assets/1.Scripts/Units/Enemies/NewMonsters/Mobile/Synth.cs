using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Synth: NewRangedEnemy {
	
	protected SynthKnockBack knockback;
	protected CacklebranchPistol gun;
	
	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		
		this.knockback = this.inventory.items[inventory.selected].GetComponent<SynthKnockBack>();
		if (this.knockback == null) Debug.LogWarning ("Synth does not have knockback equipped");
		
		foreach(KnockBackBehaviour behaviour in this.animator.GetBehaviours<KnockBackBehaviour>()) {
			behaviour.SetVar(knockback);
		}
		
		this.gun = this.gear.weapon.GetComponent<CacklebranchPistol>();
		
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
		
		this.minAtkRadius = 8.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	
	public override void SetTierData(int tier) {
		tier = 5;
		base.SetTierData (tier);
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