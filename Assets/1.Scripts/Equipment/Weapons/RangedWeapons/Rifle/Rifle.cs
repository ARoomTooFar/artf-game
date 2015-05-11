using UnityEngine;
using System.Collections;

public class Rifle : RangedWeapons {
	

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		
		stats.weapType = 5;
		stats.weapTypeName = "rifle";
		stats.damage = 20 + user.GetComponent<Character>().stats.coordination;
		stats.maxChgTime = 3;
		stats.chargeSlow = new Slow(1.0f);

		this.stats.buffDuration = 0.75f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void AttackStart() {
		this.FireProjectile();
	}
	
	public override void SpecialAttack() {
		Projectile newBullet = ((GameObject)Instantiate(this.projectile, this.transform.position + this.user.facing * 2, this.user.transform.rotation)).GetComponent<Projectile>();
		newBullet.setInitValues(user, opposition, this.stats.damage + this.stats.chgDamage, particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration * 2);
	}
	
	public override void initAttack() {
		base.initAttack();
	}
	
}
