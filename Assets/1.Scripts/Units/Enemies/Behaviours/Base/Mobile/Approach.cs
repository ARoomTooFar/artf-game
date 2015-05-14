// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class Approach : EnemyBehaviour {

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!unit.actable) return;
		this.unit.canSeePlayer(unit.target);
		this.unit.getFacingTowardsTarget();
		this.unit.rb.velocity = this.unit.facing * this.unit.stats.speed * this.unit.stats.spdManip.speedPercent;
	}
}