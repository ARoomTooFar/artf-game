// Checks lunge cd and tells when it is off 

using UnityEngine;

public class CheckLungeCD : LungeBehaviour {

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.LungeChecker(animator);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.LungeChecker(animator);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.LungeChecker(animator);
	}
	
	protected void LungeChecker(Animator animator) {
		if (lunge.curCoolDown <= 0) animator.SetTrigger("LungeOffCD");
		
		if (this.unit.target == null) return;
		
		float dis = Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position);
		if (dis >= 5) animator.SetBool ("InLungeRange", true);
		else animator.SetBool ("InLungeRange", false);
	}
}