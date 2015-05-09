// Checks lunge cd and tells when it is off 

using UnityEngine;

public class CheckSpawnCD : SpawnBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (spawn.curCoolDown <= 0 && hive.canSpawn ()) animator.SetTrigger("SpawnOffCD");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (spawn.curCoolDown <= 0 && hive.canSpawn ()) animator.SetTrigger("SpawnOffCD");
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (spawn.curCoolDown <= 0 && hive.canSpawn ()) animator.SetTrigger ("SpawnOffCD");
	}
}