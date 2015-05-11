using UnityEngine;
using System.Collections;
using System;

public class SpearExplosion : Bomb {	

	private float time;

	//-------------------//
	// Primary Functions //
	//-------------------//
	
	protected override void Start() {
		base.Start();
	}
	
	protected override void Update() {
		base.Update();
	}
	
	//---------------------//
	
	
	//------------------//
	// Public Functions //
	//------------------//

	public virtual void setInitValues(Character user, Type opposition, int damage, bool castEffect, BuffsDebuffs debuff, float time) {
		this.user = user;
		this.opposition = opposition;
		this.damage = damage;
		this.castEffect = castEffect;
		this.debuff = debuff;
		this.time = time;
		
		if (this.aoe == null) Debug.LogWarning ("AoE object not set in the inspector of a bomb");
		else {
			if (opposition == typeof(Enemy) || opposition == typeof(NewEnemy)) this.aoe.affectEnemies = true;
			if (opposition == typeof(Player)) this.aoe.affectPlayers = true;
		}


		Invoke ("explode", 0.25f);
		// this.targetsInRange = new List<Character> ();
		// if (this.castEffect && this.debuff == null) Debug.LogWarning ("Cast Effect set on bomb, but no debuff is given");
	}
	
	//-------------------//
	
	
	//---------------------//
	// Protected Functions //
	//---------------------//

	
	//---------------------//
	
	
	//-------------//
	// Inheritence //
	//-------------//
	
	protected override void onHit(Character enemy) {
		print ("Boom");
		enemy.BDS.addBuffDebuff(this.debuff, this.user.gameObject, time);
		enemy.damage(damage);
	}
	
	//-------------//

}
