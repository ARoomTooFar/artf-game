// Buff and Debuffs where a unit can have multiple instances
//     ie. A player can have multiple instances of being sped up

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stacking : BuffsDebuffs {

	protected Stacking() {
		bdType = 2;
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
}