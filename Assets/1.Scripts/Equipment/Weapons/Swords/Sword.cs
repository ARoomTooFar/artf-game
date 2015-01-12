using UnityEngine;
using System.Collections;

public class Sword : Weapons {
	public int lastChgLevel;
	public int tempDamage;
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
			lastChgLevel = (int)(stats.curChgDuration/0.4f);
			//print("Pre:" + lastChgLevel);
			print("Charge Attack power level:" + (int)(stats.curChgDuration/0.4f));
			tempDamage = stats.damage + lastChgLevel;
			//print(tempDamage);
			
		} else if (stats.curChgAtkTime == 0 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgAtkTime = Time.time;
			particles.startSpeed = 0;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			particles.startSpeed = (int)(stats.curChgDuration/0.4f);
		}
		
		if (player.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
			this.GetComponent<Collider>().enabled = false;
			lastChgLevel = 0;
			tempDamage = 0;
		}
	}
	void OnTriggerEnter(Collider other) {
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		print(tempDamage);
		if( component != null && enemy != null) {
			enemy.damage(tempDamage);
			lastChgLevel = 0;
			tempDamage = 0;
		}
	}
}
