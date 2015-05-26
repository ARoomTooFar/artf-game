using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class WeaponStats {

	// New
	public float chargeMultiplier;
	public Slow chargeSlow;

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

		this.stats.debuff = new Knockback();
		this.stats.buffDuration = 0.75f;
	}

	public virtual void equip(Character u, Type ene) {
		base.equip(u);
		u.animator.SetInteger("WeaponType", stats.weapType);
		opposition = ene;
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		// New
		stats.chargeMultiplier = 1.5f;
		stats.chargeSlow = new Slow(0.5f);

		// Keep
		stats.weapType = 0;
		stats.atkSpeed = 1.0f; // Find a better way for this maybe
		stats.damage = 5;
		stats.maxChgTime = 3;

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
		user.animator.SetFloat("ChargeTime", 0.0f);
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

	//-------------------------//
	
	public virtual IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position, 0.5f);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
}
