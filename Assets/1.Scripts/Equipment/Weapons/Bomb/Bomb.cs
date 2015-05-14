// Bomb parent class
//     For things that do a disjointed explosion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Bomb : MonoBehaviour {

	// EXPLOSIONS!!!!!!!!!!
	public GameObject expDeath;
	public AoETargetting aoe;

	// Variables
	public int damage;
	protected bool castEffect;
	protected BuffsDebuffs debuff;
	protected Type opposition;
	protected Character user;


	//-------------------//
	// Primary Functions //
	//-------------------//

	// Use this for initialization
	protected virtual void Start() {
		if (expDeath == null) Debug.LogWarning ("Explosion object not set in the inspector of a bomb");
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

		if (this.aoe == null) Debug.LogWarning ("AoE object not set in the inspector of a bomb");
		else {
			if (opposition == typeof(Enemy) || opposition == typeof(NewEnemy)) this.aoe.affectEnemies = true;
			if (opposition == typeof(Player)) this.aoe.affectPlayers = true;
		}

		// this.targetsInRange = new List<Character> ();
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
		// Create explosion while removing self
		// BombExplosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<BombExplosion>();
		if(this.damage == 0){
			Instantiate(expDeath, transform.position+new Vector3(0,.5f,0), transform.rotation);
		}else{
			Instantiate(expDeath, transform.position+new Vector3(0,2,0), transform.rotation);
		}
		Destroy (this.gameObject);

		// Variables for sight checking
		RaycastHit[] hits;
		bool inSight;
		
		// For all targets that are within our collider at this point, check that they aren't behind a wall and hit them
		foreach(Character suckers in this.aoe.unitsInRange) {//this.targetsInRange) {
			inSight = true;
			hits = Physics.RaycastAll(this.transform.position,
			                          (suckers.transform.position - this.transform.position).normalized,
			                          Vector3.Distance(this.transform.position, suckers.transform.position));

			// Check for walls
			foreach(RaycastHit hit in hits) {
				if (hit.transform.tag == "Wall") inSight = false;
			}

			if (inSight) this.onHit(suckers); // Only hits units that aren't behind walls (Add in props and other obstacles in future)
		}
	}

	protected virtual void onHit(Character enemy) {
		if(castEffect && debuff != null) {
			enemy.BDS.addBuffDebuff(debuff, this.gameObject);
		}
		enemy.damage(damage, this.user.transform);
	}

	//----------------------//


	//----------//
	// Triggers //
	//----------//


	/*
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
	}*/

	//--------------//
}
