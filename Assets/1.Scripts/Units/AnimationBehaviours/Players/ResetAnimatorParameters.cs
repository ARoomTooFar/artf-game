// Charge timer to determine how long the player is charging their attack

using UnityEngine;

public class ResetAnimatorParameters : PlayerBehaviour {
	
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("Charging", false);
		animator.SetBool ("IsInAttackAnimation", false);
		animator.SetBool ("Attack", false);
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}