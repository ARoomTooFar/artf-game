// CackleRoll state of the CackleBranch, rolls away when it is off cooldown

using UnityEngine;

public class CackleRoll : Space {

	public Roll roll;

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateExit (animator, stateInfo, layerIndex);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (animator.GetBool("Actable")) base.OnStateUpdate (animator, stateInfo, layerIndex);

		if (this.roll.curCoolDown <= 0) {
			this.roll.useItem();
			animator.SetBool ("FarEnough", true);
		}
	}
}