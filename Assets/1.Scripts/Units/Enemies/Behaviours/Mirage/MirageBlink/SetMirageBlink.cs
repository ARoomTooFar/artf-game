// Used to set animator variables for blinking
// Due to the animation startup for blink, this is used in the bahaviours and the base layer will have the animations

using UnityEngine;

public class SetMirageBlink : MirageBlinkBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetTrigger("UseBlink");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.canSeePlayer(unit.target);
		this.unit.getFacingTowardsTarget();
		if (blink.curCoolDown > 0) animator.SetBool("BlinkOffCD", false); // Used to transition out of state
	}
}
