using UnityEngine;
using System.Collections;

public class RebarSword : MeleeWeapons {

	public GameObject shockwave;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.chargeSlow = new Slow(0.0f);

		// Default sword stats
		stats.weapType = 0;
		stats.atkSpeed = 1.0f;
		stats.damage = 12;//  + user.GetComponent<Character>().stats.strength;
		stats.goldVal = 120;
		stats.maxChgTime = 3;
		
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}


	// Does something when opponent is hit
	protected override void onHit(Character enemy) {

		this.stats.buffDuration = user.animator.GetFloat ("ChargeTime") < 0.5f ? 0.75f : 1.25f;

		if(stats.debuff != null){
			if(stats.buffDuration > 0){
				enemy.BDS.addBuffDebuff(stats.debuff, this.user.gameObject, stats.buffDuration);
			}else{
				enemy.BDS.addBuffDebuff(stats.debuff, this.user.gameObject);
			}
		}
		enemy.damage(stats.damage + stats.chgDamage, user.transform, user.gameObject);
	}
}
