// Buff and Debuffs where a unit can have multiple instances
//     ie. A player can have multiple instances of being sped up

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stacking : BuffsDebuffs {

	protected Stacking() {
		bdType = 2;
	}
	
	public override void applyBD(Character unit, GameObject source) {
		base.applyBD(unit, source);
	}
	
	public override void removeBD(Character unit, GameObject source) {
		base.removeBD(unit, source);
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}