// Attacking animation of units, checks to see if animation is over for transition and parameter setting

using UnityEngine;

public class Attacking : CharacterBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("IsInAttackAnimation", true);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.normalizedTime >= 0.99f) animator.SetBool ("IsInAttackAnimation", false);
	}
}