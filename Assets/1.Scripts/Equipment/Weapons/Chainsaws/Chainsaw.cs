// Chainsaw class, put into the head for now, could possibly expand this into a special type weapon os similarity to flamethrower

using UnityEngine;
using System.Collections;

public class Chainsaw : Weapons {

	public float lastDmgTime, curDuration, maxDuration;
	private bool dealDamage;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.atkSpeed = 3.0f;
		stats.damage = (int)(1 + 0.1f * player.stats.strength);
		
		stats.maxChgTime = 3.0f;
		stats.weapType = 0;

		stats.colStart = 0.5f;
		stats.colEnd = 0.55f;
		stats.chgLevels = 0.5f;

		dealDamage = false;
		lastDmgTime = 0.0f;
		maxDuration = 5.0f;
		curDuration = 0.0f;
	}

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
	
	// Sword attack functions
	public override void attack() {
		if (!Input.GetKey(player.controls.attack) && stats.curChgAtkTime != -1) {
			stats.curChgAtkTime = -1;
		} else if (stats.curChgAtkTime == 0 && player.animSteInfo.normalizedTime > stats.colStart) {
			curDuration = maxDuration;
			stats.curChgAtkTime = Time.time;
			lastDmgTime = Time.time;
			particles.startSpeed = 0;
			this.GetComponent<Collider>().enabled = true;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;

			curDuration -= Time.deltaTime;
			if (curDuration <= 0) {
				stats.curChgAtkTime = -1;
			}
		}
		
		if (player.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
			this.GetComponent<Collider>().enabled = false;
		}
	}

	/*
	void OnTriggerEnter(Collider other) {
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage + stats.chgDamage);
		}
	}*/

	void OnTriggerStay(Collider other) {
		IDamageable<int, Vector3> component = (IDamageable<int, Vector3>) other.GetComponent( typeof(IDamageable<int, Vector3>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if(dealDamage && component != null && enemy != null) {
			enemy.damage(stats.damage, player.transform.position);
		}
	}
}
