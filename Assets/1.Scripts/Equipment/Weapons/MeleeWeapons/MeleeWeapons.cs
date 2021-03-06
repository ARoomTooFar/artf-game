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
		stats.chargeSlow = new Slow(0.0f);
	}
	
	protected override int CalculateTotalDamage() {
		return this.stats.damage + this.user.stats.strength + stats.chgDamage * this.tier;
	}

	public override void AttackStart() {
		if (action != null) StartCoroutine(makeSound(action,playSound,action.length));
		this.col.enabled = true;
	}
	
	public override void AttackEnd() {
		this.col.enabled = false;
	}

	// Does something when opponent is hit
	protected virtual void onHit(Character enemy) {
		if(stats.debuff != null){
			if(stats.buffDuration > 0){
				enemy.BDS.addBuffDebuff(stats.debuff, this.gameObject, stats.buffDuration);
			}else{
				enemy.BDS.addBuffDebuff(stats.debuff, this.gameObject);
			}
		}
		enemy.damage(this.CalculateTotalDamage(), user.transform, user.gameObject);
	}

	// only capsule collider should be checked in this function
	protected virtual void OnTriggerEnter(Collider other) {
		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			onHit(enemy);
		} else {
			IDamageable<int, Traps, GameObject> component2 = (IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
			if (component2 != null) {
				component2.damage(this.CalculateTotalDamage());
			}
		}
	}
}