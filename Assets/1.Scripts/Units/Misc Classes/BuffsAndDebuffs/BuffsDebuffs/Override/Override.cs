// Buff and Debuffs where they are overwritten by the new buff if it is better (Strength or duration wise)
//     ie. A stunned player 

using UnityEngine;
using System.Collections;

public class Override : BuffsDebuffs {
	protected Override() {
		bdType = 0;
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

	public bool overwrite(Character unit, GameObject source, BuffsDebuffs newBD) {
		if (isBetter(newBD)) {
			this.removeBD(unit, source);
			newBD.applyBD(unit, source);
			return true;
		}
		return false;
	}

	// This method is meant to be overwritten by child classes to see which buff is better
	private virtual bool isBetter(BuffsDebuffs newBD) {
		return true;
	}
}