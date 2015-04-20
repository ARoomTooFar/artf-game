// Search state of enemies, when they lost sight of there target

using UnityEngine;

public class Search : EnemyBehaviour {

	private NewMobileEnemy mUnit;

	public override void SetVar(NewEnemy unit) {
		base.SetVar (unit);
		this.mUnit = unit.GetComponent<NewMobileEnemy>();
	}

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.target = null;
		animator.SetBool ("Target", false);
		if (this.unit.lastSeenPosition.HasValue) {
			this.unit.facing = this.unit.lastSeenPosition.Value - this.unit.transform.position;
			this.unit.facing.y = 0.0f;
			this.unit.StartCoroutineEx(this.mUnit.searchForEnemy(this.unit.lastSeenPosition.Value), out this.mUnit.searchController);
			this.unit.lastSeenPosition = null;
		}
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.mUnit.StopSearch();
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}