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
		stats.damage = 4 + player.stats.strength;
		
		stats.maxChgTime = 2.0f;
		stats.weapType = 0;
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
			this.GetComponent<Collider>().enabled = true;
			print("Charge Attack power level:" + stats.chgDamage);
		} else if (stats.curChgAtkTime == 0 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgAtkTime = Time.time;
			particles.startSpeed = 0;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/0.4f);
			particles.startSpeed = stats.chgDamage;
		}
		
		if (player.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
			this.GetComponent<Collider>().enabled = false;
		}
	}
	void OnTriggerEnter(Collider other) {
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage + stats.chgDamage);
		}
	}
}
