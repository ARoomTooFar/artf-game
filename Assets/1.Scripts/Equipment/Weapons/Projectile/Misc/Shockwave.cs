using UnityEngine;
using System.Collections;
using System;

public class Shockwave : EnergyProjectile {	
	// Use this for initialization

	protected override void Start() {
		base.Start();
	}
	
	public override void setInitValues(Character player, Type opposition, int dmg,bool effect, BuffsDebuffs hinder) {
		base.setInitValues(player, opposition, dmg, effect, hinder);
		// Set stats here is each bullet will have its own properties
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}

	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			enemy.damage(damage, user);
		}
	}
}
