using UnityEngine;

public class SynthChargeBehaviour : EnemyBehaviour {
	protected Slow debuff;
	
	public virtual void SetVar (Synth unit, SynthAssaultRifle gun) {
		base.SetVar (unit.GetComponent<Enemy>());
		this.debuff = gun.stats.chargeSlow;
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.BDS.addBuffDebuff(this.debuff, this.unit.gameObject);
		animator.SetBool ("Charging", true);
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.BDS.rmvBuffDebuff(this.debuff, this.unit.gameObject);
		animator.SetBool ("Charging", false);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetFloat ("ChargeTime", animator.GetFloat("ChargeTime") + Time.deltaTime);
	}
}