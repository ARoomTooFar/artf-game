using UnityEngine;
using System.Collections;
using System;

public class Shockwave : EnergyProjectile {	
	// Use this for initialization
	protected override void Start() {
		base.Start();
	}
	
	public override void setInitValues(Character player, Type opposition, int dmg,bool effect,BuffsDebuffs hinder) {
		base.setInitValues(player, opposition, dmg, effect,hinder);
		// Set stats here is each bullet will have its own properties
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}
}
