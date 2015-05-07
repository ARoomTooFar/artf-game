// Charge timer to determine how long the player is charging their attack

using UnityEngine;

public class ChargeTimer : PlayerBehaviour {

	private int maxChargeTime;
	private Weapons weapon;

	public override void SetVar (NewPlayer unit) {
		base.SetVar (unit);
		this.weapon = unit.gear.weapon;
		this.maxChargeTime = this.weapon.stats.maxChgTime;
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat("ChargeTime", 0.0f);
		this.weapon.StartParticles();
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat ("ChargeTime", animator.GetFloat("ChargeTime") + Time.deltaTime);

		if (animator.GetFloat("ChargeTime") >= this.maxChargeTime) animator.SetBool("Charging", false); // Unleash attack when time is up
	}
}