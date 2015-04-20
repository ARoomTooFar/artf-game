// Artilitree unrooting behaviour
using UnityEngine;

public class UnRooting : EnemyBehaviour {
	
	protected float timeToUnRoot;
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.velocity = Vector3.zero;
		this.timeToUnRoot = animator.GetFloat ("Timing");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.isKinematic = false;
		animator.SetFloat("Timing", this.timeToUnRoot);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.getFacingTowardsTarget();
		
		if (this.timeToUnRoot > 0.0f) this.timeToUnRoot -= Time.deltaTime;
		else {
			this.timeToUnRoot = 0.0f;
			animator.SetFloat ("Timing", timeToUnRoot);
			animator.SetBool("Rooted", false);
		}
	}
}