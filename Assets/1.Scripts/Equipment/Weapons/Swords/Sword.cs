using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Sword : Weapons {
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		// Default sword stats
		stats.weapType = 0;
		stats.weapTypeName = "sword";
		stats.atkSpeed = 1.0f;
		stats.damage = 4 + user.GetComponent<Character>().stats.strength;
		
		stats.maxChgTime = 2.0f;

		stats.chgLevels = 0.4f;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void initAttack() {
		base.initAttack();
	}
}
