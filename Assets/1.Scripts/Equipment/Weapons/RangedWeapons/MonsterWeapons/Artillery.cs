using UnityEngine;
using System.Collections;

public class Artillery : Weapons {

	public GameObject projectile;
	protected Projectile bullet;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
	}
	
	public override void collideOn() {
	}
	
	public override void collideOff() {
	}
	
	public override void initAttack() {
		base.initAttack();
	}
	
	protected override IEnumerator bgnAttack() {
		while (user.animSteInfo.nameHash != user.atkHashCharge) {
			yield return null;
		}
	
		stats.curChgDuration = 0.0f;
		StartCoroutine(bgnCharge());
	}
	
	protected override IEnumerator bgnCharge() {
		while (user.animator.GetBool("Charging")) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			yield return null;
		}
		attack ();
	}
	
	protected override void attack() {
		base.attack ();
	}
	
	protected override void basicAttack() {
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", false);
		StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
		StartCoroutine(atkFinish());
	}
	
	protected override void chargedAttack() {
		user.GetComponent<Character>().animator.SetBool("ChargedAttack", true);
		StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
		StartCoroutine(atkFinish());
	}

	protected virtual IEnumerator Shoot() {
		yield return 0;
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
		newBullet.setInitValues(user, opposition, particles.startSpeed, user.luckCheck(), stats.debuff);
	}
}
