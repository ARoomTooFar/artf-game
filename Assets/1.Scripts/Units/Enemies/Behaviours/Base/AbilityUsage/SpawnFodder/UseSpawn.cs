// AI for using lunge, this one uses it upon entering state

using UnityEngine;

public class UseSpawn : SpawnBehaviour {
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.normalizedTime >= 0.99f) animator.SetBool("Spawn", false);
	}
}