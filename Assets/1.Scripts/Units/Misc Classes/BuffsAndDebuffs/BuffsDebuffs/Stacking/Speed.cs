// A unit under the effects of something speeding them up

using UnityEngine;
using System.Collections;

public class Speed : Stacking {
	
	private float spdPercent;

	public Speed(float speedValue) {
		name = "Speed";
		spdPercent = speedValue;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.stats.spdManip.setSpeedAmplification(spdPercent);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.spdManip.removeSpeedAmplification(spdPercent);
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}