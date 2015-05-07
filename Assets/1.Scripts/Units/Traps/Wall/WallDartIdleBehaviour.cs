using UnityEngine;

public class WallDartIdleBehaviour : StateMachineBehaviour {
	protected AoETargetting aoe;
	protected WallDarts wall;
	
	public virtual void SetVar(WallDarts wall) {
		this.aoe = wall.GetComponent<AoETargetting>();
		this.wall = wall;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.aoe.unitsInRange.Count > 0 && this.wall.isActiveAndEnabled) animator.SetBool ("Fire", true);
		else animator.SetBool ("Fire", false);
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("DoneFire", false);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}