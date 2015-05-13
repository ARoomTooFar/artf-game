using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BushyApproach : Approach {
	
	public BullCharge charge;
	private Player p;
	
	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		p = unit.target.GetComponent<Player> ();
		charge = new BullCharge ();
		unit.inventory.keepItemActive = true;
		charge.useItem();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		unit.inventory.keepItemActive = false;
	}
	
	
	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateUpdate (animator, stateInfo, layerIndex);
	}
}
