// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class Pummel : Approach {

	// public MeleeWeapons trunk;

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.velocity = Vector3.zero;
		this.unit.getFacingTowardsTarget();
		this.unit.transform.localRotation = Quaternion.LookRotation(unit.facing);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetTrigger("Attack");
		// this.unit.gear.weapon.initAttack();
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!animator.GetBool ("InAttackRange")) {
			this.unit.getFacingTowardsTarget();
			this.unit.rb.velocity = (this.unit.facing.normalized * this.unit.stats.speed * this.unit.stats.spdManip.speedPercent);
			this.unit.transform.localRotation = Quaternion.LookRotation(this.unit.facing);
		} else {
			this.unit.rb.velocity = Vector3.zero;
			animator.SetBool ("Pummeling", true);
		}
	}
}