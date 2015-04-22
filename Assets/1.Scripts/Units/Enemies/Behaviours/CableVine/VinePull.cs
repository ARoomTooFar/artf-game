using UnityEngine;

public class VinePull : Approach {
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		unit.facing = unit.target.transform.position - unit.transform.position;
		unit.facing.y = 0.0f;
		unit.target.transform.position = unit.target.transform.position - pullVelocity(0.1f);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}

	/*
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (charge.curCoolDown <= 0) animator.SetTrigger("ChargeOffCD");
		if (Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position) > this.unit.maxAtkRadius) animator.SetBool ("InChargeRange", true);
		else animator.SetBool ("InChargeRange", false);
		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}*/

	private Vector3 pullVelocity(float pull_velocity){
		float time = unit.facing.magnitude/pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = unit.facing.x / time;
		velocity.y = unit.facing.y / time;
		velocity.z = unit.facing.z / time;
		return velocity;
	}
}
