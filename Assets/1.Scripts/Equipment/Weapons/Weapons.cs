using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponStats {
	[Range(0.5f, 2.0f)]
	public float atkSpeed;
	public int damage;
	//0 -Melee, 1 -Gun, 2 -Flamethrower
	public int weapType;
	// When it actually deals damage in the animation
	public float colStart, colEnd;
	public GameObject projectile;
	//public GameObject user;
	// Charge atk variables
	public float maxChgTime, curChgAtkTime, curChgDuration;
}

public class Weapons : Equipment {

	public WeaponStats stats;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		// default weapon stats
		stats.atkSpeed = 1.0f;
		stats.damage = 5;

		stats.colStart = 0.33f;
		stats.colEnd = 0.7f;

		stats.maxChgTime = 3.0f;
		stats.curChgAtkTime = -1.0f;
		stats.curChgDuration = 0.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public virtual void initAttack() {
		stats.curChgAtkTime = stats.curChgDuration = 0;
		player.animator.SetTrigger("Attack"); // Swap over to weapon specific animation if we get some
	}

	// Weapon attack functions
	public virtual void attack() {
		if (!Input.GetKey(player.controls.attack) && stats.curChgAtkTime != -1) {
			stats.curChgAtkTime = -1;
			if(stats.weapType == 0){
				this.GetComponent<Collider>().enabled = true;
			}
			if(stats.weapType == 1){
				
				GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, player.transform.rotation);
				//Having issues passing particle.startSpeed to the bullet object..=(
				//bullet.GetComponent<Equipment>().particles.startSpeed = particles.startSpeed;
				 
			}
			print("Charge Attack power level:" + (int)(stats.curChgDuration/0.4f));
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
			if(stats.weapType == 0){
				this.GetComponent<Collider>().enabled = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage);
		}
	}
	
}
