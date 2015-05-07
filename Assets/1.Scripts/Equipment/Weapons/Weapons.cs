using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class WeaponStats {

	// New
	public float chargeMultiplier;

	// Keep
	[Range(0.5f, 2.0f)]
	public float atkSpeed; // Find a better way for this
	public PercentValues atkspdManip; // Maybe?

	[Range(1,11)]
	public int upgrade;

	public int damage;
	public int weapType;
	public BuffsDebuffs debuff;
	public float buffDuration;
	public int chgDamage, maxChgTime;

	// Old
	public string weapTypeName;
	public int chgType;
	public int specialAttackType;
	public float chgLevels, curChgAtkTime, curChgDuration, timeForChgAttack, timeForSpecial;

	public WeaponStats() {
		atkspdManip = new PercentValues();
	}
}

public class Weapons : Equipment {

	public WeaponStats stats;
	public AudioClip charge;
	public AudioClip action;
	public AudioClip chargeAttack;
	public bool playSound;
	public float soundDur;

	public Type opposition;

	protected Collider col;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		this.col = this.GetComponent<Collider>();
	}

	public virtual void equip(Character u, Type ene) {
		base.equip(u);
		u.animator.SetInteger("WeaponType", stats.weapType);
		u.weapTypeName = stats.weapTypeName;
		opposition = ene;
		// user.GetComponent<Character>().animator.SetInteger("ChargedAttackNum", stats.specialAttackType);
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		// New
		stats.chargeMultiplier = 1.5f;

		// Keep
		stats.weapType = 0;
		stats.atkSpeed = 1.0f; // Find a better way for this maybe
		stats.damage = 5;
		stats.maxChgTime = 3;

		// Old
		stats.weapTypeName = "sword";
		stats.curChgAtkTime = -1.0f;
		stats.curChgDuration = 0.0f;
		stats.chgLevels = 0.4f;

		// default weapon stats






		stats.chgDamage = 0;
		stats.timeForChgAttack = 0.8f;
		stats.timeForSpecial = 1.6f;

		soundDur = 0.1f;
		playSound = true;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	//-----------------------//
	// Some Helper Functions //
	//-----------------------//

	// Move this over to melee weapons
	public virtual void collideOn() {
		this.col.enabled = true;
	}
	
	public virtual void collideOff() {
		this.col.enabled = false;
	}


	//-----------------------//



	//----------------------------//
	// Weapon Attacking Functions //
	//----------------------------//

	// These are called by the animator
	//     For melee, these will essentiually turn the colliders on and off
	//     For ranged, these will shoot bullets, attack end probably won't do much for those
	public virtual void AttackStart () {
	}

	public virtual void AttackEnd () {
	}

	// A unique attack command called from thje animator
	//     eg. Shockwave, Ground implosion, Spray
	public virtual void SpecialAttack () {
	}

	public virtual void StartParticles () {
		particles.startSpeed = 0;
		this.StartCoroutine (BeginCharge());
	}

	public virtual void StopParticles () {
		particles.Stop();
	}

	protected virtual IEnumerator BeginCharge() {
		stats.chgDamage = 0;

		while (user.animator.GetFloat ("ChargeTime") < 0.5f && user.animator.GetBool ("Charging")) yield return null;
		if (user.animator.GetBool("Charging")) particles.Play();
		while (user.animator.GetBool("Charging")) {
			stats.chgDamage = (int) (user.animator.GetFloat ("ChargeTime") * this.stats.chargeMultiplier);
			particles.startSpeed = user.animator.GetFloat ("ChargeTime") < 0.5f ? 0 : stats.chgDamage;
			yield return null;
		}
	}



	//--------------------------------//
	// Old Weapon Attacking Functions // Remove when all the enemies have their animations implemented and all weapon using monsters are converted to new system
	//--------------------------------//


	// Start by initiateing attack animation
	public virtual void initAttack() {
		user.animator.SetTrigger("Attack");
		user.animator.speed = stats.atkSpeed * stats.atkspdManip.percentValue; // Once we have it figured out, speed can be done by animation, unless we have atacking slowing abilities
		StartCoroutine(bgnAttack());
	}

	// Once we get into the charge animation, we set our chg data and start the next co routine
	protected virtual IEnumerator bgnAttack() {
		while (user.animSteInfo.fullPathHash != user.atkHashCharge) {
			yield return null;
		}

		stats.curChgDuration = 0.0f;
		stats.chgDamage = 0;
		particles.startSpeed = 0;
		StartCoroutine(bgnCharge());
	}

	// Checks for user holding down charge sets sata accordingly
	protected virtual IEnumerator bgnCharge() {
		if (user.animator.GetBool("Charging")) particles.Play();
		while (user.animator.GetBool("Charging")) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
			yield return null;
		}
		attack ();		
	}

	// When player stops holding down charge, we check parameter for what attack to perform
	protected virtual void attack() {
		if (stats.curChgDuration >= stats.timeForSpecial) {
			user.GetComponent<Character>().animator.SetInteger("ChargedAttackNum", 1);
			chargedAttack();
		} else if (stats.curChgDuration >= stats.timeForChgAttack) {
			user.GetComponent<Character>().animator.SetInteger("ChargedAttackNum", 0);
			chargedAttack();
		} else {
			basicAttack();
		}
	}

	// Basic attack, a normal swing/stab/fire
	protected virtual void basicAttack() {
		// print("Normal Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", false);
		StartCoroutine(atkFinish());
	}

	// Charged attack, something unique to the weapon type
	protected virtual void chargedAttack() {
		// print("Charged Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", true);
		StartCoroutine(atkFinish());
	}

	// When our attack swing finishes, remove colliders, particles, and other stuff
	//     * Consider one more co routine after to check for when our animation is completely done
	protected virtual IEnumerator atkFinish() {
		while (user.animSteInfo.fullPathHash != user.atkHashEnd) {
			yield return null;
		}
		
		particles.Stop();
		
		user.animator.speed = 1.0f;
	}
	
	//-------------------------//
	
	public virtual IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
	
}
