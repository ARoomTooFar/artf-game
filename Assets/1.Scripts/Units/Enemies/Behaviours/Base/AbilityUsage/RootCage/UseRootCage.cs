// AI for using lunge, this one uses it upon entering state

using UnityEngine;

public class UseRootCage : RootCageBehaviour {
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		this.cage.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.cage.curCoolDown > 0) {
			animator.SetBool ("RootCageOffCD", false);
		}
	}
}