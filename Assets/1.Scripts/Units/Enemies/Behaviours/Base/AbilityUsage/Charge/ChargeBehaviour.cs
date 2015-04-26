// For Behaviours that need a charge reference

using UnityEngine;

public class ChargeBehaviour : EnemyBehaviour {
	
	protected BullCharge charge;
	
	public virtual void SetVar(BullCharge charge) {
		this.charge = charge;
	}
}