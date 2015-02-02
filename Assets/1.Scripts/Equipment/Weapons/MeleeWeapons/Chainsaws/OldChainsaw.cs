// Chainsaw class, put into the head for now, could possibly expand this into a special type weapon os similarity to flamethrower

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OldChainsaw : Weapons {

	public float lastDmgTime, curDuration, maxDuration;
	private bool dealDamage;
	private float slowPercent;

	private List<Character> chained;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.atkSpeed = 3.0f;
		stats.damage = (int)(1 + 0.1f * user.GetComponent<Character>().stats.strength);
		
		stats.maxChgTime = 3.0f;
		stats.weapType = 0;
		
		stats.chgLevels = 0.5f;

		dealDamage = false;
		lastDmgTime = 0.0f;
		maxDuration = 5.0f;
		curDuration = 0.0f;
		slowPercent = 0.9f;
		chained = new List<Character> ();
	}

	// Move to a Coroutine during our great weapon pur-refactor
	protected override void FixedUpdate() {
		base.FixedUpdate ();
		// Placed here for consistent Damage
		// Tells us when to deal damage
		if (stats.curChgAtkTime > 0.0f && Time.time - lastDmgTime >= 0.6f/stats.curChgDuration) {
			dealDamage = true;
			lastDmgTime = Time.time;
		} else {
			dealDamage = false;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();


	}
	
	public override void initAttack() {
		base.initAttack();
	}

	/*
	// Sword attack functions
	protected override void attack() {
		if (!Input.GetKey(user.GetComponent<Character>().controls.attack) && stats.curChgAtkTime != -1) {
			stats.curChgAtkTime = -1;
		} else if (stats.curChgAtkTime == 0 && user.GetComponent<Character>().animSteInfo.normalizedTime > stats.colStart) {
			curDuration = maxDuration;
			stats.curChgAtkTime = Time.time;
			lastDmgTime = Time.time;
			particles.startSpeed = 0;
			this.GetComponent<Collider>().enabled = true;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && user.GetComponent<Character>().animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;

			curDuration -= Time.deltaTime;
			if (curDuration <= 0) {
				stats.curChgAtkTime = -1;
			}
		}
		
		if (user.GetComponent<Character>().animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
			this.GetComponent<Collider>().enabled = false;

			if (chained.Count > 0) {
				foreach(Character meat in chained) {
					meat.removeSlow(slowPercent);
				}
				chained.Clear();
				user.removeSlow(slowPercent);
			}

		}
	}

	void OnTriggerEnter(Collider other) {
		Character enemy = other.GetComponent<Character>();
		if (enemy != null && !chained.Contains(enemy)) {
			if (chained.Count == 0) {
				user.slow (slowPercent);
			}
			chained.Add(enemy);
			enemy.slow (slowPercent);
		} 
	}

	void OnTriggerExit(Collider other) {
		Character enemy = other.GetComponent<Character>();
		if (enemy != null) {
			if (chained.Contains(enemy)) {
				chained.Remove(enemy);
				enemy.removeSlow (slowPercent);
			}

			if (chained.Count == 0) {
				user.removeSlow (slowPercent);
			}
		} 
	}

	void OnTriggerStay(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if(dealDamage && component != null && enemy != null) {
			enemy.damage(stats.damage, user);
		}
	}*/
}
