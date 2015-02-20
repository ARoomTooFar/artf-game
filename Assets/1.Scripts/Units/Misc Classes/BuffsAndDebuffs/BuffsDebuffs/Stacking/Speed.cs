// A unit under the effects of something speeding them up

using UnityEngine;
using System.Collections;

public class Speed : Stacking {
	
	private float spdPercent;

	public Speed(float speedValue) {
		name = "Speed";
		spdPercent = speedValue;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
		unit.stats.spdManip.setSpeedAmplification(spdPercent);
	}
	
	public override void removeBD(Character unit) {
		base.removeBD(unit);
		unit.stats.spdManip.removeSpeedAmplification(spdPercent);
	}

	public override void purgeBD(Character unit) {
		base.purgeBD (unit);
	}
}