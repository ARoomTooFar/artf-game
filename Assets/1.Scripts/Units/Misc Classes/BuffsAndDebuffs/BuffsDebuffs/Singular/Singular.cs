// Buff and Debuffs where only one instance can be on a unit at once
//     ie. A player can only be burned by fire once

using UnityEngine;
using System.Collections;

public class Singular : BuffsDebuffs {
	protected Singular() {
		bdType = 1;
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