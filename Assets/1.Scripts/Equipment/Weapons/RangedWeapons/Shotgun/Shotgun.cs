using UnityEngine;
using System.Collections;

public class Shotgun : RangedWeapons {

	public GameObject shockwave;

	protected int sideShockWaveAngle;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();

		this.stats.weapType = 7;
		this.stats.damage = 10;
		this.stats.maxChgTime = 4;
		
		this.spread = 30;
		this.sideShockWaveAngle = 15;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void AttackStart() {
		for (int i = 0; i < 5; i++) {
			this.FireProjectile(5, i);
		}
		StartCoroutine(makeSound(action,playSound,action.length));
	}
	
	protected virtual void FireProjectile(int maxShots, int shotNum) {
		Quaternion spreadAngle = Quaternion.AngleAxis(spread - ((spread * 2)/(maxShots - 1) * shotNum), Vector3.up); // Calculated quaternion that will rotate the bullet and its velocity
		Projectile newBullet = ((GameObject)Instantiate(projectile, this.transform.position + this.user.facing * 1, this.user.transform.rotation * spreadAngle)).GetComponent<Projectile>();
		newBullet.setInitValues(user, opposition, this.CalculateTotalDamage(), particles.startSpeed, this.stats.debuff != null, stats.debuff, this.stats.buffDuration);
		newBullet.rb.velocity =  spreadAngle * newBullet.rb.velocity;
	}
	
	public override void SpecialAttack() {
		int shots = 5 + (int)(this.user.animator.GetFloat("ChargeTime") * 3);
		for (int i = 0; i < shots; i++) {
			this.FireProjectile(shots, i);
		}
		StartCoroutine(makeSound(action,playSound,action.length));
		Shockwave wave1 = ((GameObject)Instantiate(shockwave, user.transform.position + new Vector3(0.0f, 3.0f, 0.0f), user.transform.rotation)).GetComponent<Shockwave>();
		wave1.setInitValues(user, opposition, this.CalculateTotalDamage(), false, null);
		
		if (this.user.animator.GetFloat("ChargeTime") < 2) return;
		
		Quaternion spreadAngle = Quaternion.AngleAxis(this.sideShockWaveAngle, Vector3.up);
		Shockwave wave2 = ((GameObject)Instantiate(shockwave, user.transform.position + new Vector3(0.0f, 3.0f, 0.0f), user.transform.rotation * spreadAngle)).GetComponent<Shockwave>();
		wave2.setInitValues(user, opposition, this.CalculateTotalDamage(), false, null);
		wave2.rb.velocity =  spreadAngle * wave2.rb.velocity;
		
		spreadAngle = Quaternion.AngleAxis(-this.sideShockWaveAngle, Vector3.up);
		Shockwave wave3 = ((GameObject)Instantiate(shockwave, user.transform.position + new Vector3(0.0f, 3.0f, 0.0f), user.transform.rotation * spreadAngle)).GetComponent<Shockwave>();
		wave3.setInitValues(user, opposition, this.CalculateTotalDamage(), false, null);
		wave3.rb.velocity =  spreadAngle * wave3.rb.velocity;
	}
}
