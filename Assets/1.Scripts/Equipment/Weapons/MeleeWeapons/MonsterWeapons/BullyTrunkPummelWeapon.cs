using UnityEngine;
using System.Collections;
using System;

public class BullyTrunkPummelWeapon : MeleeWeapons {
	
	// private Knockback debuff;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		
		// debuff = new Knockback();
	}
	
	public override void equip(Character u, Type ene) {
		user = u;
		setInitValues();
		opposition = ene;
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		stats.damage = (int)(user.GetComponent<Character>().stats.strength * 1.5f);
		
		soundDur = 0.1f;
		playSound = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		user.animator.SetTrigger("Attack");
	}
	
	// Does something when opponent is hit
	protected virtual void OnHit(Character enemy) {
		enemy.damage(stats.damage, user.transform, user.gameObject);
	}
	
	protected override void OnTriggerEnter(Collider other) {
		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			this.OnHit(enemy);
		} else {
			IDamageable<int, Traps, GameObject> component2 = (IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
			if (component2 == null) return;
			component2.damage(stats.damage + stats.chgDamage);
		}
	}
}
