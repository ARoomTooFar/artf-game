// Spear class, put into the head for now

using UnityEngine;
using System.Collections;

public class Spear: MeleeWeapons {

	public GameObject explosion;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.weapType = 2;
		stats.weapTypeName = "spear";
		this.stats.buffDuration = 1.25f;

		stats.atkSpeed = 1.0f;
		stats.damage = 40; // (int)(10 + 1.5f * user.GetComponent<Character>().stats.strength);
		
		stats.maxChgTime = 4;
		
		stats.chgLevels = 0.5f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}

	public override void collideOn () {
		base.collideOn ();
	}

	public override void SpecialAttack() {
		GameObject exp = (GameObject)Instantiate(this.explosion, user.transform.position, user.transform.rotation);
		exp.GetComponent<SpearExplosion>().setInitValues(user, opposition, stats.damage + stats.chgDamage, true, this.stats.debuff, 2.0f);
	}
}
