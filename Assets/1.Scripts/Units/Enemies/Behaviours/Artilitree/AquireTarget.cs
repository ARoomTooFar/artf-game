// Artilitree target behaviour
using UnityEngine;

public class AquireTarget : ArtilleryBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.curTCircle = this.artillery.curCircle;
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (Vector3.Distance(this.unit.transform.position, this.curTCircle.transform.position) > 4.0f && (this.unit.target != null && Vector3.Distance(this.curTCircle.transform.position, this.unit.target.transform.position) > 1.5f)) {
			this.curTCircle.moveCircle(this.unit.target.transform.position - this.curTCircle.transform.position);
			this.unit.getFacingTowardsTarget(); // Swap to facing target circle in future
			this.unit.transform.localRotation = Quaternion.LookRotation(unit.facing);
		} else {
			animator.SetBool("Charging", false);
		}
	}
}