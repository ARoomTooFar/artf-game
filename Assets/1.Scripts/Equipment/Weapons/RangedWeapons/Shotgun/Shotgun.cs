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
		this.stats.weapTypeName = "shotgun";
		this.stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		this.stats.maxChgTime = 4;
		
		this.spread = 40;
		this.sideShockWaveAngle = 30;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void AttackStart() {
		for (int i = 0; i < 5; i++) {
			this.FireProjectile();
		}
	}
	
	public override void SpecialAttack() {
		for (int i = 0; i < 5 + (int)(this.user.animator.GetFloat("ChargeTime") * 3); i++) {
			this.FireProjectile();
		}
		
		Shockwave wave1 = ((GameObject)Instantiate(shockwave, user.transform.position + new Vector3(0.0f, 3.0f, 0.0f), user.transform.rotation)).GetComponent<Shockwave>();
		wave1.setInitValues(user, opposition, stats.damage + stats.chgDamage, false, null);
		
		if (this.user.animator.GetFloat("ChargeTime") < 2) return;
		
		Quaternion spreadAngle = Quaternion.AngleAxis(this.sideShockWaveAngle, Vector3.up);
		Shockwave wave2 = ((GameObject)Instantiate(shockwave, user.transform.position + new Vector3(0.0f, 3.0f, 0.0f), user.transform.rotation * spreadAngle)).GetComponent<Shockwave>();
		wave2.setInitValues(user, opposition, stats.damage + stats.chgDamage, false, null);
		wave2.rb.velocity =  spreadAngle * wave2.rb.velocity;
		
		spreadAngle = Quaternion.AngleAxis(-this.sideShockWaveAngle, Vector3.up);
		Shockwave wave3 = ((GameObject)Instantiate(shockwave, user.transform.position + new Vector3(0.0f, 3.0f, 0.0f), user.transform.rotation * spreadAngle)).GetComponent<Shockwave>();
		wave3.setInitValues(user, opposition, stats.damage + stats.chgDamage, false, null);
		wave3.rb.velocity =  spreadAngle * wave3.rb.velocity;
	}
	
	public override void initAttack() {
		base.initAttack();
	}
}
