// Charge state of bully trunk, when enemy is out of attackRange it will charge BullRush

using UnityEngine;

public class ChargeAnimOver : ChargeBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.animationLock = true;
		this.unit.rb.velocity = Vector3.zero;
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool("StartCharge", false);
		// this.unit.animationLock = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.normalizedTime >= 0.99f) animator.SetTrigger ("ShouldCharge");
	}
}