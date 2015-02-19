// A unit under the effects of something slowing them down

using UnityEngine;
using System.Collections;

public class Slow : Stacking {
	
	private float spdPercent;
	
	public Slow(float speedValue) {
		spdPercent = speedValue;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
		unit.stats.spdManip.setSpeedReduction(spdPercent);
	}
	
	public override void removeBD() {
		base.removeBD();
		affectedUnit.stats.spdManip.removeSpeedReduction(spdPercent);
	}
}