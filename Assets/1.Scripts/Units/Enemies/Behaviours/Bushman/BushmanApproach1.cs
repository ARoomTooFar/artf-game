// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class BushmanApproach1 : Approach {

	protected Sprint sprint;

	public virtual void SetVar(Sprint sprint) {
		this.sprint = sprint;
	}

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//if (unit.target != null && Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius) animator.SetBool ("InChargeRange", true);
		//else animator.SetBool ("InChargeRange", false);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//if (Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius) animator.SetBool ("InChargeRange", true);
		//else animator.SetBool ("InChargeRange", false);
		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}