// CackleRoll state of the CackleBranch, rolls away when it is off cooldown

using UnityEngine;

public class BushmanSprint : Approach {
	
	protected Sprint sprint;

	public virtual void SetVar(Sprint sprint) {
		this.sprint = sprint;
	}

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		this.sprint.useItem();
		Debug.Log ("Used");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateUpdate (animator, stateInfo, layerIndex);
	}
}