// BullyTrunkBlast, since it's only used by the bully trunk and only after charging, it doesn have a cd and does not need its own item/ability behaviour hierarchy

using UnityEngine;

public class ShieldsDown : EnemyBehaviour {
	
	protected NewBullyTrunk bt;
	
	public virtual void SetVar(NewBullyTrunk bt) {
		this.bt = bt;
	}
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.bt.StartCoroutine(this.bt.shieldsDown());
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}