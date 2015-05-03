// Bully Trunk attack script to switch between its 2 attackmotions

using UnityEngine;

public class ChargedshotTimer : CharacterBehaviour {

	private float shotTimer = -5.0f;

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.shotTimer + 6 > Time.time || animator.GetInteger("Tier") < 5) animator.SetBool ("ChargedShot", false);
		else {
			animator.SetBool ("ChargedShot", true);
			this.shotTimer = Time.time;
		}
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}