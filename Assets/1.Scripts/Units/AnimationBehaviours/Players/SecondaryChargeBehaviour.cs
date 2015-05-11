// For weapons that transition into a secondary charge animation (Kneeling part of rifle)

using UnityEngine;

public class SecondaryChargeBehaviour : ChargeTimer {
	
	public override void SetVar (NewPlayer unit) {
		base.SetVar (unit);
		this.weapon = unit.gear.weapon;
		this.maxChargeTime = this.weapon.stats.maxChgTime;
		this.debuff = this.weapon.stats.chargeSlow;
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.BDS.addBuffDebuff(this.debuff, this.unit.gameObject);
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.BDS.rmvBuffDebuff(this.debuff, this.unit.gameObject);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat ("ChargeTime", animator.GetFloat("ChargeTime") + Time.deltaTime);
		
		if (animator.GetFloat("ChargeTime") >= this.maxChargeTime) animator.SetBool("Charging", false); // Unleash attack when time is up
	}
}