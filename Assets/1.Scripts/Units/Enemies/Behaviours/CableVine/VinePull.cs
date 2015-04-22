using UnityEngine;

public class VinePull : Approach {

	/*
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (charge.curCoolDown <= 0) animator.SetTrigger("ChargeOffCD");
		if (unit.target != null && Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius) animator.SetBool ("InChargeRange", true);
		else animator.SetBool ("InChargeRange", false);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (charge.curCoolDown <= 0) animator.SetTrigger("ChargeOffCD");
		if (Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius) animator.SetBool ("InChargeRange", true);
		else animator.SetBool ("InChargeRange", false);
		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}*/
}
