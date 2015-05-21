﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Synth: RangedEnemy {
	
	protected SynthKnockBack kb;
	protected CacklebranchPistol gun;
	
	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		
		this.kb = this.inventory.items[inventory.selected].GetComponent<SynthKnockBack>();
		if (this.kb == null) Debug.LogWarning ("Synth does not have knockback equipped");
		
		foreach(KnockBackBehaviour behaviour in this.animator.GetBehaviours<KnockBackBehaviour>()) {
			behaviour.SetVar(this.kb);
		}
		
		this.gun = this.gear.weapon.GetComponent<CacklebranchPistol>();
		
		this.minAtkRadius = 8.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	protected override void Update() {
		base.Update ();
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