// Checks sprint cd and tells when it is off cd
using UnityEngine;

public class CheckSprintCD : SprintBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (sprint.curCoolDown <= 0) animator.SetTrigger("SprintOffCD");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (sprint.curCoolDown <= 0) animator.SetTrigger("SprintOffCD");
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (sprint.curCoolDown <= 0) animator.SetTrigger("SprintOffCD");
	}
}
