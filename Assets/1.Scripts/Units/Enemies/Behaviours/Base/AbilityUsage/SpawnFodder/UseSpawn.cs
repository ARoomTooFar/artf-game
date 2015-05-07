// AI for using lunge, this one uses it upon entering state

using UnityEngine;

public class UseSpawn : SpawnBehaviour {
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = true;
		if(spawn.curCoolDown <= 0 && hive.canSpawn ()) this.spawn.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		this.unit.inventory.keepItemActive = false;
	}
}