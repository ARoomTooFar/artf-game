// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class MirageApproach : WatchTarget {
	
	public MirageBlink blink;
	private NewMirage mirage;

	public override void SetVar(NewEnemy unit) {
		base.SetVar(unit);
		this.mirage = unit.GetComponent<NewMirage>();
	}

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateUpdate (animator, stateInfo, layerIndex);

		if (!this.unit.stunned) {
			this.unit.facing = this.mirage.deathTarget.transform.position - this.unit.transform.position;
			this.unit.facing.y = 0.0f;
			
			if (this.blink.curCoolDown <= 0) {
				do {
					this.unit.facing.x = Random.value * (this.unit.facing.x == 0 ? (Random.value - 0.5f) : (Mathf.Abs(this.unit.facing.x)/this.unit.facing.x));
					this.unit.facing.z = Random.value * (this.unit.facing.z == 0 ? (Random.value - 0.5f) : (Mathf.Abs(this.unit.facing.z)/this.unit.facing.z));
					this.unit.facing.Normalize();
				} while (this.unit.facing == Vector3.zero);
				
				this.blink.useItem();
			}
		}
	}
}