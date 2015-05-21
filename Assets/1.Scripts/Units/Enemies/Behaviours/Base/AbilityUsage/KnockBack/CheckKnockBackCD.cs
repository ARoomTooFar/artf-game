// Checks lunge cd and tells when it is off 

using UnityEngine;

public class CheckKnockBackCD : KnockBackBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.KBChecker(animator);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.KBChecker(animator);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.KBChecker(animator);
	}
	
	protected void KBChecker(Animator animator) {
		if (this.knockback.curCoolDown <= 0) animator.SetBool("KnockBackOffCD", true);
		else animator.SetBool ("KnockBackOffCD", false);
	}
}