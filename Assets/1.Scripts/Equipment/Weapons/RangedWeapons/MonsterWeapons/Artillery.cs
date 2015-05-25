using UnityEngine;
using System.Collections;

public class Artillery : Weapons {
	public GameObject targetCircle;
	public TargetCircle curCircle;

	public GameObject projectile;
	protected ArcingBomb bullet;
	protected float curDuration;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		this.stats.weapType = 0;
		this.stats.weapTypeName = "artillery";
		
		this.stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		this.stats.maxChgTime = 3;
	}
	
	public override void collideOn() {
	}
	
	public override void collideOff() {
	}
	
	
	public virtual IEnumerator StartTargetting() {
		user.testControl = false;
		curDuration = this.stats.maxChgTime;
		Vector3 direction = user.facing.normalized * 4.0f;
		curCircle = ((GameObject)Instantiate(targetCircle, new Vector3(user.transform.position.x + direction.x, 0.55f, user.transform.position.z + direction.z), user.transform.rotation)).GetComponent<TargetCircle>();
		curCircle.setValues (this.user);
		while (user.animator.GetBool("Charging") && curDuration > 0f) {
			curDuration -= Time.deltaTime;
			yield return null;
		}
		user.animator.SetBool ("Charging", false);
	}
	
	public virtual void Shoot() {
		this.fireProjectile ();
		user.testControl = true;
	}
	
	protected void fireProjectile() {
		int damage = (int)(user.GetComponent<Character>().stats.coordination);
		this.bullet = ((GameObject)Instantiate(projectile, user.transform.position, user.transform.rotation)).GetComponent<ArcingBomb>();
		this.bullet.setInitValues(user, opposition, damage, false, null, this.curCircle.gameObject);
		this.curCircle.moveable = false;
	}
	
	
	
	protected override IEnumerator bgnAttack() {
		while (user.animSteInfo.fullPathHash != user.atkHashCharge) {
			yield return null;
		}
	
		stats.curChgDuration = 0.0f;
		StartCoroutine(bgnCharge());
	}
	
	protected override IEnumerator bgnCharge() {
		user.testControl = false;
		Vector3 direction = user.facing.normalized * 4.0f;
		// this.curTCircle.transform.position = new Vector3(this.curTCircle.transform.position.x + direction.x, this.curTCircle.transform.position.y, this.curTCircle.transform.position.z + direction.z);
		curCircle = ((GameObject)Instantiate(targetCircle, new Vector3(user.transform.position.x + direction.x, 0.55f, user.transform.position.z + direction.z), user.transform.rotation)).GetComponent<TargetCircle>();
		curCircle.setValues (this.user);
		while (user.animator.GetBool("Charging") && stats.curChgDuration < stats.maxChgTime) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			yield return null;
		}
		user.animator.SetBool ("Charging", false);
		attack ();
	}

	protected override void attack() {
		this.Shoot ();
	}

	protected override IEnumerator atkFinish() {
		while (user.animSteInfo.fullPathHash != user.atkHashEnd) {
			yield return null;
		}
		
	}
	

}
