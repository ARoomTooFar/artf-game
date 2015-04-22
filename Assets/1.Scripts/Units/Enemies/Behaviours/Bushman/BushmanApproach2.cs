// AI approach state, when they have a target, this is when they move towards their target if they are out of range

using UnityEngine;

public class BushmanApproach2 : BushmanApproach1 {

	protected BullCharge charge;
	
	protected override void SetTriggers(Animator animator) {
		base.SetTriggers(animator);
		if (charge.curCoolDown <= 0) animator.SetTrigger("ChargeOffCD");
	}
}