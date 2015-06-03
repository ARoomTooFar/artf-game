// Ranged weapon class
//     For weapons that fire out a projectile

using UnityEngine;
using System.Collections;

public class RangedWeapons : Weapons {

	public GameObject projectile;
	protected int spread;
	
	protected Quaternion spray;
	protected float variance;
	protected float kick;
	protected Projectile bullet;
	
	protected bool needReload;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		
		this.spread = 15;
	}
	
	protected virtual int CalculateTotalDamage() {
		return this.stats.damage + this.user.stats.coordination + stats.chgDamage * (this.tier + 1);
	}

	public override void AttackStart() {
		this.StartCoroutine(this.Shoot(1));
	}
	
	public override void AttackEnd() {
	}


	public override void collideOn() {
	}
	
	public override void collideOff() {
	}

	protected virtual IEnumerator Shoot(int count) {
		yield return 0;
	}

	protected IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}
	
	protected virtual void FireProjectile() {
		Quaternion spreadAngle = Quaternion.AngleAxis(Random.Range (-this.spread, this.spread), Vector3.up); // Calculated quaternion that will rotate the bullet and its velocity
		Projectile newBullet = ((GameObject)Instantiate(projectile, this.transform.position + this.user.facing * 2, this.user.transform.rotation * spreadAngle)).GetComponent<Projectile>();
		newBullet.setInitValues(user, opposition, this.CalculateTotalDamage(), particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration);
		newBullet.rb.velocity =  spreadAngle * newBullet.rb.velocity;
	}
}