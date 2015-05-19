// Sets the stats of the enemies when they start up
using UnityEngine;

public class SetStats : EnemyBehaviour {

	public Stats stat = new Stats();

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.SetInitValues (
			this.stat.health * (animator.GetInteger("Tier") + 1),
			this.stat.strength * (animator.GetInteger("Tier") + 1),
			this.stat.coordination * (animator.GetInteger("Tier") + 1),
			this.stat.armor + this.stat.armorBonus * (animator.GetInteger("Tier") + 1),
			this.stat.speed * (animator.GetInteger("Tier") + 1)
		);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}