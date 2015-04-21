// Artilitree rooting behaviour
using UnityEngine;

public class Rooting : EnemyBehaviour {
	
	protected float rootTime;
	protected float timeToRoot;
	
	public virtual void SetVar (float rootTime) {
		this.rootTime = rootTime;
	}
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.rb.isKinematic = true;
		this.unit.rb.velocity = Vector3.zero;
		this.timeToRoot = animator.GetFloat ("Timing");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat ("Timing", timeToRoot);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.getFacingTowardsTarget();
		
		if (this.timeToRoot < this.rootTime) this.timeToRoot += Time.deltaTime;
		else {
			this.timeToRoot = this.rootTime;
			animator.SetFloat ("Timing", timeToRoot);
			animator.SetBool("Rooted", true);
		}
	}
}