using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponStats {
	[Range(0.5f, 2.0f)]
	public float atkSpeed;
	public int damage;
	//counts number of hits so far in the multiple hit string
	public int multHit;
	//0 -Melee, 1 -Gun, 2 -Flamethrower
	public int weapType;
	// When it actually deals damage in the animation
	public float colStart, colEnd;
	public GameObject projectile;
	//This will be used to implement abilities in the spread sheet since different weapons have different effects when charged up. 
	//For now~ Using 1 as shoot a powerful singular shot, using 2 for a line of three shots (For gun, but base case 1 is same animation more powerful damage)
	public int chgType;
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
		stats.multHit = 0;
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
		;
	}

	void OnTriggerEnter(Collider other) {
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage);
		}
	}
}
