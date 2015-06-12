using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Artilitree: RangedEnemy {
	
	public GameObject AE;
	
	protected RootRing roots;
	protected RootCage cage;
	protected Artillery artillery;

	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		
		this.minAtkRadius = 4.0f;
		this.maxAtkRadius = 40.0f;
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void SetTierData(int tier) {
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
		
		if (tier == 6) {
			this.inventory.cycItems();
			this.cage = this.inventory.items[inventory.selected].GetComponent<RootCage>();
			if (this.cage == null) Debug.LogWarning ("Artilitree does not have root cage equipped");
			this.cage.eUser = this.GetComponent<Enemy>();
			
			foreach(RootCageBehaviour behaviour in this.animator.GetBehaviours<RootCageBehaviour>()) {
				behaviour.SetVar(this.cage);
			}
		}
		
		base.SetTierData(tier);
		
		monsterLoot.initializeLoot("Artilitree", tier);
	}
	
	public override void SetInitValues(int health, int strength, int coordination, int armor, float speed) {
		base.SetInitValues(health, strength, coordination, armor, speed);
	
		this.artillery = this.gear.weapon.GetComponent<Artillery>();
		if (this.artillery == null) print("Artilitree has no artillery equipped");
		
		foreach(ArtilleryBehaviour behaviour in this.animator.GetBehaviours<ArtilleryBehaviour>()) {
			behaviour.SetVar(this.artillery);
		}
	}
	
	
	protected virtual void StartTargetting() {
		this.StartCoroutine(this.artillery.StartTargetting());
	}
	
	protected virtual void Shoot() {
		this.artillery.Shoot();
	}
	
	//---------------------//
	// Character Functions //
	//---------------------//


	public override void die() {
		if (this.tier > 3) {
			ArtilitreeExplosion aEx = ((GameObject)Instantiate(this.AE, this.transform.position, Quaternion.identity)).GetComponent<ArtilitreeExplosion>();
			aEx.setInitValues(this, this.opposition, this.artillery.stats.damage, false, null, 0f);
			// animator.SetTrigger("Attack");
			// StartCoroutine(waitTillDeath());
		}
		base.die ();
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