// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class SynthSetCharge : EnemyBehaviour {
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.unit.target == null || !this.unit.canSeePlayer(this.unit.target)) animator.SetBool ("ChargedShot", false);
		else animator.SetBool ("ChargedShot", Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius/4);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.unit.target == null || !this.unit.canSeePlayer(this.unit.target)) animator.SetBool ("ChargedShot", false);
		else animator.SetBool ("ChargedShot", Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius/4);
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.unit.target == null || !this.unit.canSeePlayer(this.unit.target)) animator.SetBool ("ChargedShot", false);
		else animator.SetBool ("ChargedShot", Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius/4);
	}
}