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
		if(user.luckCheck() && stats.debuff != null){
			if(stats.buffDuration > 0){
				enemy.BDS.addBuffDebuff(stats.debuff, this.gameObject, stats.buffDuration);
			}else{
				enemy.BDS.addBuffDebuff(stats.debuff, this.gameObject);
			}
		}
		enemy.damage(stats.damage + stats.chgDamage, user);
	}

	// only capsule collider should be checked in this function
	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			onHit(enemy);
		} else {
			IDamageable<int, Traps> component2 = (IDamageable<int, Traps>) other.GetComponent (typeof(IDamageable<int, Traps>));
			if (component2 != null) {
				component2.damage(stats.damage + stats.chgDamage);
			}
		}
	}
}