using UnityEngine;
using System.Collections;

public class Sword : Weapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		// Default sword stats
		stats.atkSpeed = 1.0f;
		stats.damage = 4;
		
		stats.maxChgTime = 2.0f;
		stats.weapType = 0;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void initAttack() {
		base.initAttack();
	}

	// Sword attack functions
	public override void attack() {
		base.attack ();
	}
}
