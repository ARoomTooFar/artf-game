using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Synth: RangedEnemy {
	
	protected SynthKnockBack kb;
	protected SynthAssaultRifle gun;
	
	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		
		this.minAtkRadius = 5.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	
	protected override void SetTierData(int tier) {
		if (tier > 2) {
			this.kb = this.inventory.items[inventory.selected].GetComponent<SynthKnockBack>();
			if (this.kb == null) Debug.LogWarning ("Synth does not have knockback equipped");
			this.kb.eUser = this.GetComponent<Enemy>();
			
			foreach(KnockBackBehaviour behaviour in this.animator.GetBehaviours<KnockBackBehaviour>()) {
				behaviour.SetVar(this.kb);
			}
		}
		
		base.SetTierData (tier);
	}
	
	public override void SetInitValues(int health, int strength, int coordination, int armor, float speed) {
		base.SetInitValues(health, strength, coordination, armor, speed);
		this.gun = this.gear.weapon.GetComponent<SynthAssaultRifle>();
		
		if (this.tier == 5) {
			foreach(SynthChargeBehaviour behaviour in this.animator.GetBehaviours<SynthChargeBehaviour>()) {
				behaviour.SetVar(this, this.gun);
			}
		}
	}
	
	
	//----------------------//
	// Transition Functions //
	//----------------------//
	
	//----------------------//
	
	
	//-------------------//
	// Actions Functions //
	//-------------------//
	
	public virtual void Shoot() {
		this.gun.AttackStart();
	}
	
	public virtual void SprayAndPray() {
		this.animator.SetBool("Charging", true);
		this.gun.SpecialAttack();
	}

	public override void die() {
		this.isDead = true;
		deathNoise ();
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