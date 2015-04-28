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
		stats.maxHealth = 200;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=2;
		stats.luck=0;

		this.minAtkRadius = 4.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	public override void SetTierData(int tier) {
		tier = 0;
		base.SetTierData(tier);
		
		if (tier < 1) stats.speed = 2;
		else if (tier > 0 && tier < 5) stats.speed = 3;
		else stats.speed = 4;
		
		foreach(Rooting behaviour in this.animator.GetBehaviours<Rooting>()) {
			if (tier < 3) behaviour.SetVar(3);
			else behaviour.SetVar(2);
		}
		
		if (tier > 4) {
			this.roots = this.inventory.items[inventory.selected].GetComponent<RootRing>();
			if (this.roots == null) Debug.LogWarning ("Artilitree does not have root ring equipped");
			
			foreach(RootSelf behaviour in this.animator.GetBehaviours<RootSelf>()) {
				behaviour.SetVar(this.roots);
			}
		}
	}
	
	//---------------------//
	// Character Functions //
	//---------------------//

	public override void die() {
		if (this.tier > 3) {
			this.artillery.initAttack();
			StartCoroutine(waitTillDeath());
		} else {
			base.die ();
		}
	}
	
	//---------------------//

	//------------//
	// Coroutines //
	//------------//

	protected virtual IEnumerator waitTillDeath() {
		while (this.artillery.curCircle == null) {
			yield return null;
		}
		base.die ();
	}
	
	
	//------------//
	
	


	
	void OnCollisionEnter(Collision collision) {
		if (collision.collider.name == "Root(Clone)") { // Possibly change this if needed
			collision.gameObject.GetComponent<Prop>().die ();
		}
	}
}