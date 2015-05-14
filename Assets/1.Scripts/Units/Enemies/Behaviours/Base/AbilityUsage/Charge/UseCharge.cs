// Charge state of bully trunk, when enemy is out of attackRange it will charge BullRush

using UnityEngine;

public class UseCharge : ChargeBehaviour {
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		this.charge.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		float dis = Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position);
		if (dis <= this.charge.curChgTime * 3 || dis >= 22 || this.charge.curChgTime >= this.charge.maxChgTime || !animator.GetBool("Target")) {
			animator.SetBool("StartCharge", true);
		}
	}
}