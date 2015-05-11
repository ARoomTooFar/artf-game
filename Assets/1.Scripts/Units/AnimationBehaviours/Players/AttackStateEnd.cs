// Charge timer to determine how long the player is charging their attack

using UnityEngine;

public class AttackStateEnd : PlayerBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!animator.GetBool ("Charging")) animator.SetBool ("Attack", false);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}