

using UnityEngine;

public class MirageBlinkStrike : EnemyBehaviour {

	protected BlinkStrike blink;

	public virtual void SetVar(BlinkStrike blink) {
		this.blink = blink;
	}

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.unit.target == null) return;
		this.unit.facing = this.unit.target.transform.position - this.unit.transform.position;
		this.unit.facing.y = 0.0f;
		this.blink.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}
