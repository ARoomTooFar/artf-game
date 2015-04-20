// AtkAnimation state of enemies, the rest state while the swing of the attack animation goes down

using UnityEngine;

public class InAtkAnimation : EnemyBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.velocity = Vector3.zero;
	}
}