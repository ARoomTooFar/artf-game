// Checks blink cd and tells when it is off cd

using UnityEngine;

public class CheckMirageBlinkCD : MirageBlinkBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("HitEnemy", false);
		if (blink.curCoolDown <= 0) animator.SetBool("BlinkOffCD", true);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// if (blink.curCoolDown <= 0) animator.SetTrigger("BlinkOffCD");
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (blink.curCoolDown <= 0) animator.SetBool("BlinkOffCD", true);
	}
}
