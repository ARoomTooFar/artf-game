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
		
		this.kb = this.inventory.items[inventory.selected].GetComponent<SynthKnockBack>();
		if (this.kb == null) Debug.LogWarning ("Synth does not have knockback equipped");
		this.kb.eUser = this.GetComponent<Enemy>();
		
		foreach(KnockBackBehaviour behaviour in this.animator.GetBehaviours<KnockBackBehaviour>()) {
			behaviour.SetVar(this.kb);
		}
		
		this.minAtkRadius = 5.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	protected override void Update() {
		base.Update ();
		
		if (this.target != null && this.canSeePlayer(this.target)) {
			float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
			this.animator.SetBool ("ChargedShot", distance > this.maxAtkRadius/4);
		}
	}
	
	
	public override void SetTierData(int tier) {
		tier = 5;
		base.SetTierData (tier);
	}
	
	public override void SetInitValues(int health, int strength, int coordination, int armor, float speed) {
		base.SetInitValues(health, strength, coordination, armor, speed);
		this.gun = this.gear.weapon.GetComponent<SynthAssaultRifle>();
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