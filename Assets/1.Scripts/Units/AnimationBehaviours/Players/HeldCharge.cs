// For weapons whose charge function needs to be held down for the greatest effect (Chainsaw and assault rifle)

using UnityEngine;

public class HeldCharge : ChargeTimer {
	
	private int maxChargeTime;
	private Weapons weapon;
	private Slow debuff;
	
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
	}
}