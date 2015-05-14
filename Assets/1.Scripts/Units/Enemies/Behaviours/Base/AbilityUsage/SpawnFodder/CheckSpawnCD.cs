// Checks lunge cd and tells when it is off 

using UnityEngine;

public class CheckSpawnCD : SpawnBehaviour {
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.CheckSpawn(animator);
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.CheckSpawn(animator);
	}
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.CheckSpawn(animator);
	}
	
	public void CheckSpawn(Animator animator) {
		if (spawn.curCoolDown <= 0 && hive.canSpawn ()) animator.SetBool ("Spawn", true);
	}
}