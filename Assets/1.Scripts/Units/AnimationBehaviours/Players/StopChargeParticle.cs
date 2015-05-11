// Charge timer to determine how long the player is charging their attack

using UnityEngine;

public class StopChargeParticle : PlayerBehaviour {

	private Weapons weapon;
	
	public override void SetVar (Player unit) {
		base.SetVar (unit);
		this.weapon = unit.gear.weapon;
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.weapon.StopParticles();
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}