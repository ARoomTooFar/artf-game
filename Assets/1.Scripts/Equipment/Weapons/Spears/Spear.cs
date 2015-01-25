// Spear class, put into the head for now

using UnityEngine;
using System.Collections;

public class Spear : Weapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.atkSpeed = 1.0f;
		stats.damage = (int)(2 + 1.5f * player.stats.strength);
		
		stats.maxChgTime = 2.0f;
		stats.weapType = 0;

		stats.colStart = 0.4f;
		stats.colEnd = 0.6f;
		stats.chgLevels = 0.5f;
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
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
		}
		
		if (player.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
			this.GetComponent<Collider>().enabled = false;
		}
	}
	void OnTriggerEnter(Collider other) {
		IDamageable<int, Vector3> component = (IDamageable<int, Vector3>) other.GetComponent( typeof(IDamageable<int, Vector3>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage + stats.chgDamage, player.transform.position);
		}
	}
}
