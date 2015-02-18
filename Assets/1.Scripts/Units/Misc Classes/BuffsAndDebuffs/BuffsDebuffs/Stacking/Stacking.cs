// Buff and Debuffs where a unit can have multiple instances
//     ie. A player can have multiple instances of being sped up

using UnityEngine;
using System.Collections;

public class Stacking : BuffsDebuffs {
	protected Stacking() {
		bdType = 2;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
	}
	
	public override void removeBD() {
		base.removeBD();
	}
}