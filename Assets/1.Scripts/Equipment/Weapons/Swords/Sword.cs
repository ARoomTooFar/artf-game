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
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	// Sword attack functions
	protected override void attack() {
		base.attack ();
	}
}
