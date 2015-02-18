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
	
	}
	
	public override void removeBD() {
		base.removeBD();
	}
}