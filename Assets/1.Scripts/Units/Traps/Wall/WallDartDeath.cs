using UnityEngine;
using System.Collections;

public class WallDartDeath : StateMachineBehaviour {
	protected AoETargetting aoe;
	protected WallDarts wall;
	
	public virtual void SetVar(WallDarts wall) {
		this.aoe = wall.GetComponent<AoETargetting>();
		this.wall = wall;
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		wall.deathSound();
	}
}