// Bomb parent class
//     For things that do a disjointed explosion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bomb : MonoBehaviour {

	// Variables
	protected int damage;
	protected bool castEffect;
	protected BuffsDebuffs debuff;
	protected Type opposition;
	protected Character user;

	protected List<Character> targetsInRange;

	//-------------------//
	// Primary Functions //
	//-------------------//

	// Use this for initialization
	protected virtual void Start() {

	}
	
	// Update is called once per frame
	protected virtual void Update() {
	}

	protected virtual void setInitValues(Character user, Type opposition, int damage, bool castEffect, BuffsDebuffs debuff) {
		this.user = user;
		this.opposition = opposition;
		this.damage = damage;
		this.castEffect = castEffect;
		this.debuff = debuff;
		
		this.targetsInRange = new List<Character> ();
		if (this.castEffect && this.debuff == null) Debug.LogWarning ("Cast Effect set on bomb, but no debuff is given");
	}

	//-------------------------//


	//------------------//
	// Public Functions //
	//------------------//



	//------------------//


	//---------------------//
	// Explosion Functions //
	//---------------------//

	protected virtual void explode() {
		foreach(Character suckers in this.targetsInRange) {
			this.onHit(suckers);
		}
	}

	protected virtual void onHit(Character enemy) {
		if(castEffect && debuff != null) {
			enemy.BDS.addBuffDebuff(debuff, this.gameObject);
		}
		enemy.damage(damage, user);
	}

	//----------------------//


	//----------//
	// Triggers //
	//----------//

	void OnTriggerEnter(Collider other) {
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null) {
			this.targetsInRange.Add(enemy);
		}
	}

	void OnTriggerExit(Collider other) {
		Character enemy = (Character)other.GetComponent (opposition);
		if (enemy != null) {
			this.targetsInRange.Remove(enemy);
		}
	}

	//--------------//
}
