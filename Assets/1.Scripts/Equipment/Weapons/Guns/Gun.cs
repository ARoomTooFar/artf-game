using UnityEngine;
using System.Collections;

public class Gun : Weapons {
	public string bullPattern;
	
	
	// Use this for initialization
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		stats.atkSpeed = 2.0f;
		stats.damage = 1;
		stats.multHit = 3;
		stats.chgType = 2;
		stats.maxChgTime = 2.0f;
		stats.weapType = 1;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}

	public override void attack() {
		base.attack ();
	}
}
