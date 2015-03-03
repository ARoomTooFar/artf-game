using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class WeaponStats {
	[Range(0.5f, 2.0f)]
	public float atkSpeed;
	[Range(1,11)]
	public int upgrade;
	public int damage;
	//counts number of hits so far in the multiple hit string
	public int multHit;


	public int weapType;
	public string weapTypeName;
	public BuffsDebuffs debuff;
	public float buffDuration;

	//This will be used to implement abilities in the spread sheet since different weapons have different effects when charged up. 
	//For now~ Using 1 as shoot a powerful singular shot, using 2 for a line of three shots (For gun, but base case 1 is same animation more powerful damage)
	public int chgType;
	// Charge atk variables
	public int chgDamage;
	public float maxChgTime, chgLevels, curChgAtkTime, curChgDuration, timeForChgAttack, timeForSpecial;
	public int specialAttackType;
}

public class Weapons : Equipment {

	public WeaponStats stats;
	public AudioClip charge;
	public AudioClip action;
	public bool playSound;
	public float soundDur;

	public Type opposition;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	public virtual void equip(Character u, Type ene) {
		base.equip(u);
		u.animator.SetInteger("WeaponType", stats.weapType);
		u.weapTypeName = stats.weapTypeName;
		opposition = ene;
		user.GetComponent<Character>().animator.SetInteger("ChargedAttackNum", stats.specialAttackType);
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		// default weapon stats
		stats.weapType = 0;
		stats.weapTypeName = "sword";
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
		playSound = true;
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	//-----------------------//
	// Some Helper Functions //
	//-----------------------//

	public virtual void collideOn() {
		this.GetComponent<Collider>().enabled = true;
	}
	
	public virtual void collideOff() {
		this.GetComponent<Collider>().enabled = false;
	}

	// A unique attack command called from thje animator
	//     eg. Shockwave
	public virtual void specialAttack() {
	}

	//-----------------------//

	//----------------------------//
	// Weapon Attacking Functions //
	//----------------------------//


	// Start by initiateing attack animation
	public virtual void initAttack() {
		user.animator.SetTrigger("Attack");
		user.animator.speed = stats.atkSpeed; // Once we have it figured out, speed can be done by animation, unless we have atacking slowing abilities
		StartCoroutine(bgnAttack());
	}

	// Once we get into the charge animation, we set our chg data and start the next co routine
	protected virtual IEnumerator bgnAttack() {
		while (user.animSteInfo.nameHash != user.atkHashCharge) {
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
		while (user.animSteInfo.nameHash != user.atkHashEnd) {
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
