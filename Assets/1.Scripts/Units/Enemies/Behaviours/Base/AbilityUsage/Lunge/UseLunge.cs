// AI for using lunge, this one uses it upon entering state

using UnityEngine;

public class UseLunge : LungeBehaviour {
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		this.lunge.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("LungeOffCD", false);
		this.unit.inventory.keepItemActive = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.unit.target == null) return;
		
		float dis = Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position);
		if (dis >= 10) {
			animator.SetTrigger("ShouldCharge");
		}
	}
}