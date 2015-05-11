using UnityEngine;
using System.Collections;

public class Pistol : RangedWeapons {

	public GameObject explosiveRound;

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
		
		this.spread = 0;// Pistols have no spread
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void AttackStart() {
		this.FireProjectile();
	}

	public override void SpecialAttack() {
		ExplosiveRound newBullet = ((GameObject)Instantiate(this.explosiveRound, this.transform.position + this.user.facing * 2, this.user.transform.rotation)).GetComponent<ExplosiveRound>();
		newBullet.setInitValues(user, opposition, this.stats.damage + this.stats.chgDamage, particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration * 3);
	}

	public override void initAttack() {
		base.initAttack();
	}
}
