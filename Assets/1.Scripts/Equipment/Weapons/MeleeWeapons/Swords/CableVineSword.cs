using UnityEngine;
using System.Collections;

public class CableVineSword : Sword {

	protected Stun stunDebuff;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		stunDebuff = new Stun ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		//base.setInitValues();
		
		//stats.damage = 4;
		base.setInitValues();
		stats.weapType = 0;
		stats.specialAttackType = 0;
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

	protected override void onHit(Character enemy) {
		base.onHit (enemy);
		enemy.BDS.addBuffDebuff (stunDebuff, this.gameObject, 4.0f);
	}
}
