// Buff and Debuffs where they are overwritten by the new buff if it is better (Strength or duration wise)
//     ie. A stunned player 

using UnityEngine;
using System.Collections;

public class Override : BuffsDebuffs {
	protected Override() {
		bdType = 0;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}

	// This method is meant to be overwritten by child classes to see which buff is better
	//     * Start time is the Time.time when it started, lifeTime is how long it has been on the unit
	public virtual bool isBetter(BuffsDebuffs newBD, float newDuration, float timeLeft) {
		return true;
	}
}