// Unit will be unable to act when stunned
using UnityEngine;
using System.Collections;

public class Stun : Override {
	
	public Stun() {
		name = "Stun";
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.stun();
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.removeStun();
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
		unit.removeStun();
	}

	public override bool isBetter(BuffsDebuffs newBD, float newDuration, float timeLeft) {
		if (newDuration > timeLeft) {
			return true;
		}
		return false;
	}
}