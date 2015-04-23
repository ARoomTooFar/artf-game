// BullyTrunkBlast, since it's only used by the bully trunk and only after charging, it doesn have a cd and does not need its own item/ability behaviour hierarchy

using UnityEngine;

public class TrunkBlast : EnemyBehaviour {

	protected BullyTrunkBlast blast;

	public virtual void SetVar(BullyTrunkBlast blast) {
		this.blast = blast;
	}

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.blast.useItem();
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}
}