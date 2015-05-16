// Checks lunge cd and tells when it is off 

using UnityEngine;

public class CheckRootCageCD : RootCageBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.CageChecker(animator);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.CageChecker(animator);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.CageChecker(animator);
	}
	
	protected void CageChecker(Animator animator) {
		if (cage.curCoolDown <= 0) animator.SetBool("RootCageOffCD", true);
		else animator.SetBool ("RootCageOffCD", false);
	}
}