using UnityEngine;

public class SpikeTrapIdleBehaviour : StateMachineBehaviour {
	protected AoETargetting aoe;
	protected SpikeTrap spike;
	
	public virtual void SetVar(SpikeTrap spike) {
		this.aoe = spike.GetComponent<AoETargetting>();
		this.spike = spike;
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (this.aoe.unitsInRange.Count > 0) animator.SetBool ("Fire", true);
		else animator.SetBool ("Fire", false);
	}
	
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetBool ("DoneFire", false);
		this.spike.RaiseSpike();
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	}
}