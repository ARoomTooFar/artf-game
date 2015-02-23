// Buff and Debuffs where only one instance can be on a unit at once
//     ie. A player can only be burned by fire once

using UnityEngine;
using System.Collections;

public class Singular : BuffsDebuffs {
	protected Singular() {
		bdType = 1;
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