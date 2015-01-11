using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponStats {
	[Range(0.5f, 2.0f)]
	public float atkSpeed;
	public int damage;

	// When it actually deals damage in the animation
	public float colStart, colEnd;

	// Charge atk variables
	public float maxChgTime, curChgAtkTime, curChgDuration;
}

public class Weapons : Equipment {

	public WeaponStats stats;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		// default weapon stats
		stats.atkSpeed = 1.0f;
		stats.damage = 5;

		stats.colStart = 0.33f;
		stats.colEnd = 0.7f;

		stats.maxChgTime = 3.0f;
		stats.curChgAtkTime = -1.0f;
		stats.curChgDuration = 0.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	// Weapon attack functions
	protected virtual void attack() {
	}
}
