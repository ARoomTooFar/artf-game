// Dagger class, put into the head for now

using UnityEngine;
using System.Collections;

public class Shiv : MeleeWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.weapType = 1;
		stats.atkSpeed = 2.0f;
		stats.damage = (int)(10);
		this.stats.buffDuration = 0.25f;
		stats.goldVal = 100;
		stats.maxChgTime = 2;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void collideOn () {
		base.collideOn ();
		/*
		if (stats.chgDamage-- == 0) { // Our multistab counter
			user.GetComponent<Character>().animator.SetBool("ChargedAttack", false);
		}*/
	}

	protected override void onHit(Character enemy) {
		if(stats.debuff != null && user.animator.GetFloat ("ChargeTime") < 0.5f && stats.buffDuration > 0) {
			enemy.BDS.addBuffDebuff(stats.debuff, this.user.gameObject, stats.buffDuration);
		}

		if (ARTFUtilities.IsBehind(user.transform.position, enemy.facing, enemy.transform.position)) {
			enemy.damage((int)((stats.damage + stats.chgDamage) * 1.5f), user.transform, user.gameObject);
		} else {
			enemy.damage(stats.damage + stats.chgDamage, user.transform, user.gameObject);
		}

	}
}
