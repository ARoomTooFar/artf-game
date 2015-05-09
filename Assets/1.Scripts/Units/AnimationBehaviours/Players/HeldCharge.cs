// For weapons whose charge function needs to be held down for the greatest effect (Chainsaw and assault rifle)

using UnityEngine;

public class HeldCharge : ChargeTimer {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.BDS.addBuffDebuff(this.debuff, this.unit.gameObject);
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.BDS.rmvBuffDebuff(this.debuff, this.unit.gameObject);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}