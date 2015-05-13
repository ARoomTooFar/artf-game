using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CVRetract : EnemyBehaviour {

	private Transform transform;
	private CVSensor feeler;

	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		feeler = unit.GetComponentInChildren<CVSensor> ();
		transform = unit.transform.Find ("CVFeelers");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
	
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

	}

}
