using UnityEngine;
using System.Collections;

public class AssaultRifle : RangedWeapons {
	
	protected int baseSpread, maxSpread;
	protected float lastFireTime, curDuration, lastDmgTime;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		this.stats.weapType = 6;
		this.stats.weapTypeName = "assaultRifle";
		
		this.stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		this.stats.maxChgTime = 5;
		
		this.spread = 10;
		this.maxSpread = 60;
		this.baseSpread = spread;
		this.lastFireTime = Time.time;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void AttackStart() {
		particles.startSpeed = 0;
		if ((Time.time - lastFireTime) > 1) this.spread = this.baseSpread;
		else this.spread = Mathf.Clamp(this.spread + this.baseSpread, baseSpread, this.maxSpread);
		this.FireProjectile();
	}
	
	public override void SpecialAttack() {
		this.StartCoroutine(this.SprayAndPray());
	}
	
	protected virtual IEnumerator SprayAndPray() {
		this.user.lockRotation = true;
		curDuration = this.stats.maxChgTime;
		lastDmgTime = Time.time;
		while(user.animator.GetBool("Charging") && curDuration > 0) {
			StartCoroutine(makeSound(action,playSound,action.length));
			stats.chgDamage = (int) (user.animator.GetFloat ("ChargeTime") * this.stats.chargeMultiplier);
			particles.startSpeed = stats.chgDamage;
			curDuration -= Time.deltaTime;
			
			if (Time.time - lastDmgTime >= curDuration/(this.stats.maxChgTime * 4)) {
				lastDmgTime = Time.time;
				this.FireProjectile();
			}
			
			yield return null;
		}
		user.animator.SetBool("Charging", false);
		this.user.lockRotation = false;
	}
	
	public override void initAttack() {
		base.initAttack();
	}
}
