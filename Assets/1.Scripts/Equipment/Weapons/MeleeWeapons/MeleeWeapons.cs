// Melee weapon class
//     For weapons that hit closeup

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MeleeWeapons : Weapons {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
	}
	
	public override void initAttack() {
		base.initAttack();
	}

	// Does something when opponent is hit
	protected virtual void onHit(Character enemy) {
		enemy.damage(stats.damage + stats.chgDamage, user);
	}

	// only capsule collider should be checked in this function
	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			onHit(enemy);
		}
	}
}