using UnityEngine;
using System.Collections;

public class SixShooter : RangedWeapons {

	public GameObject explosiveRound;
	
	public GameObject radar;
	protected AoETargetting aoe;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();

		this.stats.weapType = 4;
		this.stats.weapTypeName = "pistol";
		this.stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		this.stats.maxChgTime = 5;
		this.stats.buffDuration = 0.4f;
		this.stats.goldVal = 100;
		this.spread = 0;// Pistols have no spread
		
		GameObject dar = (GameObject) Instantiate(radar, this.user.transform.position + radar.transform.position, Quaternion.identity);
		dar.transform.parent = this.user.transform;
		this.aoe = dar.GetComponent<AoETargetting>();
		this.aoe.affectEnemies = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void AttackStart() {
		StartCoroutine(makeSound(action,playSound,action.length));
		this.FireProjectile();
	}

	public override void SpecialAttack() {
		StartCoroutine(makeSound(chargeAttack,playSound,chargeAttack.length));
		ExplosiveRound newBullet = ((GameObject)Instantiate(this.explosiveRound, this.transform.position + this.user.facing * 2, this.user.transform.rotation)).GetComponent<ExplosiveRound>();
		newBullet.setInitValues(user, opposition, this.stats.damage + this.stats.chgDamage, particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration * 3, this.user.FindClosestCharacter(this.aoe.unitsInRange));
	}

	public override void initAttack() {
		base.initAttack();
	}
	
	protected override void FireProjectile() {
		HomingBullet newBullet = ((GameObject)Instantiate(projectile, this.transform.position + this.user.facing * 2, this.user.transform.rotation)).GetComponent<HomingBullet>();
		newBullet.setInitValues(user, opposition, this.stats.damage + this.stats.chgDamage, particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration, this.user.FindClosestCharacter(this.aoe.unitsInRange));
	}
}
