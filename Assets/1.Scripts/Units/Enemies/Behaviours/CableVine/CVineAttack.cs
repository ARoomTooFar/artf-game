using UnityEngine;

public class CVineAttack : Attack {
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (unit.actable && !unit.attacking){
			unit.gear.weapon.initAttack();
		}
	}
}
