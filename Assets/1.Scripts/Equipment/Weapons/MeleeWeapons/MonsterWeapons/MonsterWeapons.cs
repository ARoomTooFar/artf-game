using UnityEngine;
using System.Collections;
using System;


public class MonsterWeapons : Weapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	public override void equip(Character u, Type ene) {
		user = u;
		setInitValues();
		opposition = ene;
	}
	
	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// default weapon stats
		stats.weapType = 0;
		stats.atkSpeed = 1.0f;
		stats.damage = 5;
		stats.multHit = 0;
		
		stats.maxChgTime = 3.0f;
		stats.curChgAtkTime = -1.0f;
		stats.curChgDuration = 0.0f;
		stats.chgLevels = 0.4f;
		stats.chgDamage = 0;
		stats.timeForChgAttack = 0.8f;
		stats.timeForSpecial = 1.6f;
		stats.specialAttackType = 0;
		soundDur = 0.1f;
		playSound = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	//-----------------------//
	// Some Helper Functions //
	//-----------------------//
	
	public override void collideOn() {
		this.col.enabled = true;
	}
	
	public override void collideOff() {
		this.col.enabled = false;
	}

	public override void specialAttack() {
	}
	
	//-----------------------//
	
	//----------------------------//
	// Weapon Attacking Functions //
	//----------------------------//
	
	
	// Start by initiateing attack animation
	public override void initAttack() {
		user.animator.SetTrigger("Attack");
		// user.animator.speed = stats.atkSpeed * stats.atkspdManip.percentValue; // Once we have it figured out, speed can be done by animation, unless we have attack slowing abilities
	}

	//-------------------------//
	
	public override IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
	
}
