using UnityEngine;
using System.Collections;

public class UseKnockBack : KnockBackBehaviour {

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		if(this.knockback.curCoolDown <= 0)
			this.knockback.useItem ();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}

	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(this.knockback.curCoolDown <= 0)
			this.knockback.useItem ();
	}
}
