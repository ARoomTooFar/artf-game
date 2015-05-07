// Chainsaw class, put into the head for now, could possibly expand this into a special type weapon os similarity to flamethrower

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chainsaw : MeleeWeapons {
	
	public float lastDmgTime, curDuration, maxDuration;
	private bool dealDamage;
	private float slowPercent;
	
	private Dismember debuff;

	protected class Dismember : Stacking {
		
		private float spdPercent, redPercent;
		
		public Dismember(float speedValue) {
			name = "Dismember";
			particleName = "BloodParticles";
			spdPercent = speedValue;
		}
		
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			newData.unit.stats.spdManip.setSpeedReduction(spdPercent);
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.stats.spdManip.removeSpeedReduction(spdPercent);
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}

	private List<Character> chained;
	private List<GameObject> cropping;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		// User dagger vars for now until we have chainsaw animations
		stats.weapType = 2;
		stats.weapTypeName = "chainsaw";

		stats.atkSpeed = 3.0f;
		stats.damage = (int)(5 + 0.1f * user.GetComponent<Character>().stats.strength);
		
		stats.maxChgTime = 3;
		stats.timeForChgAttack = 0.5f;
		
		stats.chgLevels = 0.5f;

		lastDmgTime = 0.0f;
		maxDuration = 5.0f;
		curDuration = 0.0f;
		slowPercent = 0.9f;
		debuff = new Dismember(slowPercent);
		chained = new List<Character> ();
		cropping = new List<GameObject>();
	}

	// Move to a Coroutine during our great weapon pur-refactor
	protected override void FixedUpdate() {
		base.FixedUpdate ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}

	protected override IEnumerator bgnAttack() {
		return base.bgnAttack();
	}

	// Chainsaw attacks in a unique way
	protected override IEnumerator bgnCharge() {
		if (user.animator.GetBool("Charging")) particles.Play();
		while (user.animator.GetBool("Charging") && stats.curChgDuration < stats.timeForChgAttack) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
			yield return null;
		}
		attack ();
	}

	protected override void attack() {
		base.attack ();
	}

	protected override void basicAttack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.animator.SetBool("ChargedAttack", false);
		this.GetComponent<Collider>().enabled = true;
		StartCoroutine(atkFinish());
	}

	protected override void chargedAttack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.animator.SetBool("ChargedAttack", true);
		this.GetComponent<Collider>().enabled = true;
		StartCoroutine(dismember());
		StartCoroutine(atkFinish());
	}

	// Logic for when we have our chainsaw out and sawing some bitch ass plants
	protected virtual IEnumerator dismember() {
		curDuration = maxDuration;
		lastDmgTime = Time.time;
		while(user.animator.GetBool("Charging") && curDuration > 0) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
			curDuration -= Time.deltaTime;

			if (Time.time - lastDmgTime >= 0.6f/stats.curChgDuration) {
				lastDmgTime = Time.time;
				foreach(Character meat in chained) {
					if (meat == null) continue;
					meat.damage(stats.damage, user.transform, user.gameObject);
				}

				foreach(GameObject crop in cropping) {
					if (crop == null) continue;
					IDamageable<int, Traps, GameObject> component = (IDamageable<int, Traps, GameObject>) crop.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
					if(component != null) {
						component.damage(stats.damage);
					}
				}
			}

			yield return null;
		}
		user.animator.SetBool("ChargedAttack", false);
	}
	
	// When our attack swing finishes, remove colliders, particles, and other stuff
	//     * Consider one more co routine after to check for when our animation is completely done
	protected override IEnumerator atkFinish() {
		while (user.animSteInfo.fullPathHash != user.atkHashEnd) {
			yield return null;
		}
		
		particles.Stop();
		this.GetComponent<Collider>().enabled = false;
		if (chained.Count > 0) {
			foreach(Character meat in chained) {
				meat.BDS.rmvBuffDebuff(debuff, user.gameObject);
			}
			chained.Clear();

		}

		user.BDS.rmvBuffDebuff(debuff, user.gameObject);

		cropping.Clear();

		user.animator.speed = 1.0f;
	}

	void OnTriggerEnter(Collider other) {
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null && !chained.Contains(enemy)) {

			if (user.animator.GetBool("Charging")) {
				if (chained.Count == 0) {
					user.BDS.addBuffDebuff(debuff, user.gameObject);
				}
				chained.Add(enemy);
				enemy.BDS.addBuffDebuff(debuff, user.gameObject);
			} else {
				enemy.damage(stats.damage * 2, user.transform, user.gameObject);
			}
		} else {
			IDamageable<int, Traps, GameObject> component = (IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
			if (component != null) {
				if (user.animator.GetBool("Charging")) {
					cropping.Add(other.gameObject);
				} else {
					component.damage(stats.damage * 2);
				}
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null) {
			if (chained.Contains(enemy)) {
				chained.Remove(enemy);
				enemy.BDS.rmvBuffDebuff(debuff, user.gameObject);
			}
			
			if (chained.Count == 0 && user.animator.GetBool("Charging")) {
				user.BDS.rmvBuffDebuff(debuff, user.gameObject);
			}
		} else {
			IDamageable<int, Traps, GameObject> component = (IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
			if (component != null) {
				if (cropping.Contains (other.gameObject)) {
					cropping.Remove(other.gameObject);
				}
			}
		}
	}
}
