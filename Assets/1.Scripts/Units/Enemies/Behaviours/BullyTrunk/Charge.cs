// Charge state of bully trunk, when enemy is out of attackRange it will charge BullRush

using UnityEngine;

public class Charge : EnemyBehaviour {

	public BullCharge charge;

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		this.charge.useItem();
		animator.SetBool("ChargeOffCD", false);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.getFacingTowardsTarget();
		this.unit.rb.velocity = (this.unit.facing.normalized * this.unit.stats.speed * this.unit.stats.spdManip.speedPercent);
		this.unit.transform.localRotation = Quaternion.LookRotation(this.unit.facing);

		float dis = Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position);
		if (dis <= this.charge.curChgTime * 2.5 || dis >= 16 || this.charge.curChgTime >= this.charge.maxChgTime) {
			animator.SetTrigger("ShouldCharge");
		}
	}
}