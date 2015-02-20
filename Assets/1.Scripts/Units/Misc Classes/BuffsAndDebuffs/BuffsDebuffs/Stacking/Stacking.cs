// Buff and Debuffs where a unit can have multiple instances
//     ie. A player can have multiple instances of being sped up

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stacking : BuffsDebuffs {

	protected Stacking() {
		bdType = 2;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
	}
	
	public override void removeBD(Character unit) {
		base.removeBD(unit);
	}

	public override void purgeBD(Character unit) {
		base.purgeBD (unit);
	}
}