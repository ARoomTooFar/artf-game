// A unit under the effects of something speeding them up

using UnityEngine;
using System.Collections;

public class Speed : Stacking {
	
	private float spdPercent;

	public Speed(float speedValue) {
		spdPercent = speedValue;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
		unit.stats.spdManip.setSpeedAmplification(spdPercent);
	}
	
	public override void removeBD() {
		base.removeBD();
		affectedUnit.stats.spdManip.removeSpeedAmplification(spdPercent);
	}
}