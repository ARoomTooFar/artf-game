// Buff and Debuffs where only one instance can be on a unit at once
//     ie. A player can only be burned by fire once

using UnityEngine;
using System.Collections;

public class Singular : BuffsDebuffs {
	protected Singular() {
		bdType = 1;
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