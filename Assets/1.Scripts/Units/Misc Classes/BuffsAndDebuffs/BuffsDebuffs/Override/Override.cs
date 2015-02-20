using UnityEngine;
using System.Collections;

public class Override : BuffsDebuffs {
	protected Override() {
		bdType = 0;
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
