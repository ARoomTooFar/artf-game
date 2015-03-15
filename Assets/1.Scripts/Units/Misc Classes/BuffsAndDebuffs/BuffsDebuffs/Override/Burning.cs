// Once hit with fire (Fire pit, flamethrower, etc) unit will be burning
//     Small DOT on unit for a short time, 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Burning : Override {

	private int dmg;

	public Burning(int damage) {
		name = "Burn";
		dmg = damage;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.StartCoroutineEx(burnBabyBurn(newData.unit), out newData.controller);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.controller.Stop ();
	}

	private IEnumerator burnBabyBurn(Character unit) {
		while (true) {
			unit.damage(dmg);

			yield return new WaitForSeconds(1.0f);
		}
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}

	public override bool isBetter(BuffsDebuffs newBD, float newDuration, float timeLeft) {
		if (newDuration - 1.0f > timeLeft) {
			return true;
		}
		return false;
	}
}