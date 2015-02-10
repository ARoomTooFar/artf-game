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
	public int currAmmo;
	public int maxAmmo;
	protected float loadSpeed;
	protected bool reload;
	
	protected Projectile bullet;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected override void setInitValues() {
		base.setInitValues();
		reload = false;
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
		print("Normal Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", false);
		StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
		StartCoroutine(atkFinish());
	}

	protected override void chargedAttack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", true);
		StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
		StartCoroutine(atkFinish());
	}

	protected virtual IEnumerator Shoot(int count) {
		yield return 0;
	}

	protected IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}
	protected virtual IEnumerator loadAmmo(){
		yield return StartCoroutine(Wait(loadSpeed));
		if(reload){
			currAmmo = maxAmmo;
			reload = false;
		}
	}

	protected override IEnumerator atkFinish() {
		while (user.animSteInfo.nameHash != user.atkHashEnd) {
			yield return null;
		}
		particles.Stop();
		
		user.animator.speed = 1.0f;
	}

	protected void fireProjectile() {
		Projectile newBullet = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Projectile>();
		newBullet.setInitValues(user, opposition, particles.startSpeed);
	}
}