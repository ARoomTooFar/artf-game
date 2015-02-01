using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponStats {
	[Range(0.5f, 2.0f)]
	public float atkSpeed;
	[Range(1,11)]
	public int upgrade;
	public int damage;
	//counts number of hits so far in the multiple hit string
	public int multHit;
	//0 -Melee, 1 -Gun, 2 -Flamethrower
	public int weapType;
	public string weapTypeName;
	// When it actually deals damage in the animation
	public float colStart, colEnd;
	public GameObject projectile;
	//This will be used to implement abilities in the spread sheet since different weapons have different effects when charged up. 
	//For now~ Using 1 as shoot a powerful singular shot, using 2 for a line of three shots (For gun, but base case 1 is same animation more powerful damage)
	public int chgType;
	//public GameObject user;
	// Charge atk variables
	public int chgDamage;
	public float maxChgTime, chgLevels, curChgAtkTime, curChgDuration;
}

public class Weapons : Equipment {

	public WeaponStats stats;
	public AudioClip charge;
	public AudioClip action;
	public bool playSound;
	public float soundDur;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	public virtual void equip(Character u) {
		base.equip(u);
		u.animator.SetInteger("WeaponType", stats.weapType);
		u.weapTypeName = stats.weapTypeName;
	}

	// Used for setting stats for each weapon piece
	protected override void setInitValues() {
		base.setInitValues();

		// default weapon stats
		stats.weapType = 0;
		stats.weapTypeName = "sword";
		stats.atkSpeed = 1.0f;
		stats.damage = 5;
		stats.multHit = 0;
		stats.colStart = 0.33f;
		stats.colEnd = 0.7f;

		stats.maxChgTime = 3.0f;
		stats.curChgAtkTime = -1.0f;
		stats.curChgDuration = 0.0f;
		stats.chgLevels = 0.4f;
		stats.chgDamage = 0;
		soundDur = 0.1f;
		playSound = true;
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	//----------------------------//
	// Weapon Attacking Functions //
	//----------------------------//
	
	public virtual void initAttack() {
		user.animator.SetTrigger("Attack"); // Swap over to weapon specific animation if we get some
		user.animator.speed = stats.atkSpeed;
		StartCoroutine(bgnAttack());
	}
	
	protected IEnumerator bgnAttack() {
		while (user.animSteInfo.nameHash != user.atkHashCharge) {
			yield return null;
		}

		stats.curChgDuration = 0.0f;
		stats.chgDamage = 0;
		particles.startSpeed = 0;
		StartCoroutine(bgnCharge());
	}

	// If things need to be done while charging make this virtual 
	protected IEnumerator bgnCharge() {
		if (user.gear.charging) particles.Play();
		while (user.gear.charging) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
			yield return null;
		}
		
		if (stats.curChgAtkTime >= 0.5f) {
			chargedAttack();
		} else {
			attack ();
		}
	}

	protected virtual void attack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetTrigger("ChargeDone");
		this.GetComponent<Collider>().enabled = true;
		StartCoroutine(atkFinish());
	}
	
	protected virtual void chargedAttack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetTrigger("ChargeDone");
		this.GetComponent<Collider>().enabled = true;
		StartCoroutine(atkFinish());
	}
	
	protected IEnumerator atkFinish() {
		while (user.animator.GetCurrentAnimatorStateInfo(0).nameHash != user.atkHashEnd) {
			yield return null;
		}
		
		particles.Stop();
		this.GetComponent<Collider>().enabled = false;
		
		user.animator.speed = 1.0f;
	}
	
	//-------------------------//
	
	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage + stats.chgDamage, user);
		}
	}
	
	public virtual IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
	
}
