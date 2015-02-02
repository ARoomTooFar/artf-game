using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Sword : Weapons {
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		// Default sword stats
		stats.atkSpeed = 1.0f;
		stats.damage = 4 + user.GetComponent<Character>().stats.strength;
		
		stats.maxChgTime = 2.0f;
		stats.weapType = 0;

		stats.chgLevels = 0.4f;
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
		if (!Input.GetKey(user.GetComponent<Character>().controls.attack) && stats.curChgAtkTime != -1) {
			stats.curChgAtkTime = -1;
			this.GetComponent<Collider>().enabled = true;
			print("Charge Attack power level:" + stats.chgDamage);
		} else if (stats.curChgAtkTime == 0 && user.GetComponent<Character>().animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgAtkTime = Time.time;
			particles.startSpeed = 0;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && user.GetComponent<Character>().animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
		}
		
		if (user.GetComponent<Character>().animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
			this.GetComponent<Collider>().enabled = false;
		}
	}
	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage + stats.chgDamage, user);
		}
	}
}
