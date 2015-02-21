// A unit under the effects of something slowing them down

using UnityEngine;
using System.Collections;

public class Slow : Stacking {
	
	private float spdPercent;
	
	public Slow(float speedValue) {
		name = "Slow";
		spdPercent = speedValue;
	}
	
	public override void applyBD(Character unit, GameObject source) {
		base.applyBD(unit, source);
		unit.stats.spdManip.setSpeedReduction(spdPercent);
	}
	
	public override void removeBD(Character unit, GameObject source) {
		base.removeBD(unit, source);
		unit.stats.spdManip.removeSpeedReduction(spdPercent);
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}