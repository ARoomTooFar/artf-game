// Artilitree target behaviour
using UnityEngine;

public class AquireTarget : ArtilleryBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.curTCircle = this.artillery.curCircle;
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool("Charging", false);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// Checks to see if circle is within targetting unit
		if (!(this.unit.target != null && Vector3.Distance(this.curTCircle.transform.position, this.unit.target.transform.position) > 1.5f))
			animator.SetBool("Charging", false);
		this.curTCircle.moveCircle(this.unit.target.transform.position - this.curTCircle.transform.position);
		this.unit.getFacingTowardsTarget(); // Swap to facing target circle in future
	}
}