// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class SynthStrafe : EnemyBehaviour {
	
	//protected Character tar;
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//this.tar = this.unit.target.GetComponent<Character>();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!unit.actable) return;
		if (this.unit.target == null) return;
		this.unit.canSeePlayer(unit.target);
		
		Vector3 vectorOfSight = (this.unit.transform.position - this.unit.target.transform.position).normalized;
		vectorOfSight.y = 0.0f;
		if (vectorOfSight == this.unit.facing) return;
		
		float dis = Vector3.Distance(this.unit.transform.position, this.unit.target.transform.position);
		
		Vector3 positionToBe = this.unit.target.transform.position - (this.unit.facing * dis);
		
		this.unit.rb.velocity = (positionToBe - this.unit.transform.position).normalized * this.unit.stats.speed * this.unit.stats.spdManip.speedPercent;
	}
}