// Dagger class, put into the head for now

using UnityEngine;
using System.Collections;

public class Dagger : MeleeWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.weapType = 1;
		stats.weapTypeName = "dagger";
		stats.atkSpeed = 2.0f;
		stats.damage = (int)(2 + 1f * user.GetComponent<Character>().stats.strength);
		
		stats.maxChgTime = 1.0f;

		stats.chgLevels = 0.2f;
		stats.timeForChgAttack = 0.2f;
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
		if (stats.chgDamage-- == 0) { // Our multistab counter
			user.GetComponent<Character>().animator.SetBool("ChargedAttack", false);
		}
	}
}
