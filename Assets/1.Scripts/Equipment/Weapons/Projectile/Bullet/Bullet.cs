using UnityEngine;
using System.Collections;
using System;

public class Bullet : Projectile {	
	// Use this for initialization
	protected override void Start() {
		base.Start();
	}

	public override void setInitValues(Character player, Type opposition, int damage, float partSpeed, bool effect, BuffsDebuffs hinder, float durations) {
		base.setInitValues(player, opposition, damage, partSpeed, effect, hinder, duration);
		// Set stats here is each bullet will have its own properties
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}
}
