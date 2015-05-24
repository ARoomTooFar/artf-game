// Space state of enemies, atm it imitates their random search state
//     Once pathfinding is in, this can be when they reach the end of a patrol path or just the patrol path

using UnityEngine;

public class Space : EnemyBehaviour {

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.velocity = Vector3.zero;
		this.unit.facing = this.unit.facing * -1;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.facing = (this.unit.lastSeenPosition.Value - this.unit.transform.position) * -1;
		this.unit.facing.y = 0.0f;
		this.unit.MoveForward();
		// this.unit.rb.velocity = this.unit.facing * this.unit.stats.speed * this.unit.stats.spdManip.speedPercent;
	}
}