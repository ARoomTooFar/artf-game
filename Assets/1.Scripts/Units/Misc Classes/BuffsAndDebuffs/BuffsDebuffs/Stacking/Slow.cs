// A unit under the effects of something slowing them down

using UnityEngine;
using System.Collections;

public class Slow : Stacking {
	
	private float spdPercent;
	
	public Slow(float speedValue) {
		name = "Slow";
		spdPercent = speedValue;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.stats.spdManip.setSpeedReduction(spdPercent);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.spdManip.removeSpeedReduction(spdPercent);
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}