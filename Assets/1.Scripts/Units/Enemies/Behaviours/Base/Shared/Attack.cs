// Attack state of enemies, when in the desired range the unit will initiate an attack

using UnityEngine;

public class Attack : EnemyBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.velocity = Vector3.zero;
		this.unit.getFacingTowardsTarget();
		this.unit.transform.localRotation = Quaternion.LookRotation(unit.facing);
		this.unit.gear.weapon.initAttack();
		this.unit.gear.weapon.collideOn();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.gear.weapon.collideOff();
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}