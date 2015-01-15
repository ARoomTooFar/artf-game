using UnityEngine;
using System.Collections;

public class TestDagger : Dagger {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		//base.setInitValues();
		
		//stats.damage = 4;
		base.setInitValues();
		stats.weapType = 0;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}
	
	// Test sword attack functions
	public override void attack() {
		base.attack ();
	}
}
