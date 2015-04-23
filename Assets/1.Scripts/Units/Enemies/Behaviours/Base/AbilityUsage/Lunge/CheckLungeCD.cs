// Checks lunge cd and tells when it is off 

using UnityEngine;

public class CheckLungeCD : LungeBehaviour {

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (lunge.curCoolDown <= 0) animator.SetTrigger("LungeOffCD");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (lunge.curCoolDown <= 0) animator.SetTrigger("LungeOffCD");
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (lunge.curCoolDown <= 0) animator.SetTrigger("LungeOffCD");
	}
}