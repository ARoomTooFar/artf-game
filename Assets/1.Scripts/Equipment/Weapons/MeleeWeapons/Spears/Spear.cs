// Spear class, put into the head for now

using UnityEngine;
using System.Collections;

public class Spear: MeleeWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.weapType = 1; // Use dagger animation for now
		stats.weapTypeName = "dagger";

		stats.atkSpeed = 1.0f;
		stats.damage = (int)(2 + 1.5f * user.GetComponent<Character>().stats.strength);
		
		stats.maxChgTime = 2.0f;
		
		stats.chgLevels = 0.5f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}
}
