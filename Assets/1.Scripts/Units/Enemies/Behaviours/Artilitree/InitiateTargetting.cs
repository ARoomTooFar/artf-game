// Artilitree initiating his attack sequence
using UnityEngine;

public class InitiateTargetting : ArtilleryBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.getFacingTowardsTarget();
		this.unit.transform.localRotation = Quaternion.LookRotation(this.unit.facing);
		// this.artillery.initAttack();
		animator.SetTrigger("Attack");
		animator.SetBool("Charging", true);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.artillery.curCircle != null) {
			animator.SetBool ("GotCircle", true);
		}
	}
}