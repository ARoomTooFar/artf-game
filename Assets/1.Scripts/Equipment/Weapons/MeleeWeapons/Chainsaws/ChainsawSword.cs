// Chainsaw class, put into the head for now, could possibly expand this into a special type weapon os similarity to flamethrower

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainsawSword : MeleeWeapons {
	
	public float lastDmgTime, curDuration, maxDuration;
	protected bool dealDamage;
	protected float slowPercent;
	
	protected Dismember debuff;

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

	protected List<Character> chained;
	protected List<GameObject> cropping;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		// User dagger vars for now until we have chainsaw animations
		stats.weapType = 3;
		stats.goldVal = 80;
		stats.atkSpeed = 3.0f;
		stats.damage = 8;
		
		stats.maxChgTime = 7;

		lastDmgTime = 0.0f;
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

	public override void SpecialAttack() {
		particles.startSpeed = 0;
		StartCoroutine(makeSound(action,playSound,action.length));
		this.StartCoroutine (this.dismember());
	}

	protected virtual IEnumerator dismember() {
		curDuration = this.stats.maxChgTime;
		lastDmgTime = Time.time;
		while(user.animator.GetBool("Charging") && curDuration > 0) {
			stats.chgDamage = (int) (user.animator.GetFloat ("ChargeTime") * this.stats.chargeMultiplier);
			particles.startSpeed = stats.chgDamage;
			curDuration -= Time.deltaTime;

			if (Time.time - lastDmgTime >= curDuration/(this.stats.maxChgTime)) {
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
		user.animator.SetBool("Charging", false);
		if (chained.Count > 0) {
			foreach(Character meat in chained) {
				meat.BDS.rmvBuffDebuff(debuff, user.gameObject);
			}
			chained.Clear();	
		}
		cropping.Clear ();
		particles.startSpeed = 0;
	}

	protected override void OnTriggerEnter(Collider other) {
		if (!user.animator.GetBool("Charging")) {
			base.OnTriggerEnter(other);
			return;
		}
		
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null && !chained.Contains(enemy)) {
			chained.Add(enemy);
			enemy.BDS.addBuffDebuff(debuff, user.gameObject);
		} else {
			IDamageable<int, Traps, GameObject> component = (IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
			if (component != null) {
				cropping.Add(other.gameObject);
			}
		}
	}
	
	protected virtual void OnTriggerExit(Collider other) {
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null) {
			if (chained.Remove(enemy)) enemy.BDS.rmvBuffDebuff(debuff, user.gameObject);
		} else {
			if ((IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>)) != null) {
				cropping.Remove(other.gameObject);
			}
		}
	}
}
