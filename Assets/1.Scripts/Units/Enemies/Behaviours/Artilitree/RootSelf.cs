// Artilitree rooting behaviour
using UnityEngine;

public class RootSelf : EnemyBehaviour {
	
	protected RootRing roots;
	
	public virtual void SetVar (RootRing roots) {
		this.roots = roots;
	}
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.roots.curCoolDown <= 0) this.roots.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}