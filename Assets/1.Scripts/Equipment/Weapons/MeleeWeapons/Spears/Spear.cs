// Spear class, put into the head for now

using UnityEngine;
using System.Collections;

public class Spear: MeleeWeapons {

	public GameObject explosion;

	protected Stun stunD;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		
		this.stunD = new Stun();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.chargeSlow = new Slow(1.0f);

		stats.weapType = 2;
		this.stats.buffDuration = 1.25f;

		stats.atkSpeed = 1.0f;
		stats.damage = 40; // (int)(10 + 1.5f * user.GetComponent<Character>().stats.strength);
		
		stats.maxChgTime = 4;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	

	public override void collideOn () {
		base.collideOn ();
	}
	
	public override void AttackStart() {
		this.col.enabled = true;
		this.StartCoroutine(Rush ());
	}
	
	protected override void onHit(Character enemy) {
		if(stats.debuff != null){
			if(user.animator.GetFloat ("ChargeTime") < 0.5f){
				enemy.BDS.addBuffDebuff(stats.debuff, this.user.gameObject, stats.buffDuration);
			} else {
				enemy.BDS.addBuffDebuff(this.stunD, this.user.gameObject, 1.5f);
			}
		}
		enemy.damage(stats.damage + stats.chgDamage, user.transform, user.gameObject);
	}

	/*
	public override void SpecialAttack() {
		GameObject exp = (GameObject)Instantiate(this.explosion, user.transform.position, user.transform.rotation);
		exp.GetComponent<SpearExplosion>().setInitValues(user, opposition, stats.damage + stats.chgDamage, true, this.stats.debuff, 2.0f);
	}*/
	
	protected IEnumerator Rush() {
		Vector3 facing = this.user.facing;
		Vector3 spd = this.user.facing * (user.animator.GetFloat ("ChargeTime") < 0.5f ? 25f : 30f + (10f * user.animator.GetFloat ("ChargeTime")/4));
		while (facing == this.user.facing && spd.magnitude > 0.5f) {
			this.user.rb.velocity = spd;
			spd = spd - (spd.normalized * (Time.deltaTime * 75f));
			yield return null;
		}
	}
}
