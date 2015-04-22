// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class BushmanApproach3 : BushmanApproach2 {

	protected Lunge lunge;
	
	protected override void SetTriggers(Animator animator) {
		base.SetTriggers(animator);
		if (lunge.curCoolDown <= 0) animator.SetTrigger ("LungeOffCD");
	}
}