using UnityEngine;
using System.Collections;

public class BullyTrunkWeapon : MeleeWeapons {
	
	private Knockback debuff;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		
		debuff = new Knockback();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {

		base.setInitValues();
		stats.damage = (int)(3 + 0.25f * user.GetComponent<Character>().stats.strength);;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}
	
	// Test sword attack functions
	protected override void attack() {
		base.attack ();

	}
	// Does something when opponent is hit
	protected override void onHit(Character enemy) {
		// base.onHit (enemy);
		enemy.damage(stats.damage + stats.chgDamage, user.transform, user.gameObject);
	}
}
