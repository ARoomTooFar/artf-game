// Ranged weapon class
//     For weapons that fire out a projectile

using UnityEngine;
using System.Collections;

public class RangedWeapons : Weapons {

	public GameObject projectile;
	//for inaccuracy

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
	}

	public override void AttackStart() {
		this.StartCoroutine(this.Shoot(1));
	}
	
	public override void AttackEnd() {
		// this.col.enabled = false;
	}


	public override void collideOn() {
	}
	
	public override void collideOff() {
	}

	public override void initAttack() {
		base.initAttack();
	}

	protected override IEnumerator bgnAttack() {
		return base.bgnAttack();
	}

	protected override IEnumerator bgnCharge() {
		return base.bgnCharge();
	}

	protected override void attack() {
		base.attack ();
	}

	protected override void basicAttack() {
		// print("Normal Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", false);
		StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
		StartCoroutine(makeSound(action,playSound,action.length));
		StartCoroutine(atkFinish());
	}

	protected override void chargedAttack() {
		// print("Charged Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", true);
		StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
		StartCoroutine(makeSound(chargeAttack,playSound,chargeAttack.length));
		StartCoroutine(atkFinish());
	}

	protected virtual IEnumerator Shoot(int count) {
		yield return 0;
	}

	protected IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}


	protected override IEnumerator atkFinish() {
		while (user.animSteInfo.fullPathHash != user.atkHashEnd) {
			yield return null;
		}
		particles.Stop();
		user.animator.speed = 1.0f;
	}
	
	protected void fireProjectile() {
		Projectile newBullet = ((GameObject)Instantiate(projectile, this.transform.position + this.user.facing * 2, this.user.transform.rotation)).GetComponent<Projectile>();
		newBullet.setInitValues(user, opposition, this.stats.damage, particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration);
	}
}