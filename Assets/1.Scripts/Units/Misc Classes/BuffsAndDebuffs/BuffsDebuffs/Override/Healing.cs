// Once hit with the Healing Slime
//     Small HOT on unit for a short time, 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Healing : Override {

	private int healPow;

	public Healing(int power) {
		name = "Heal";
		healPow = power;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.StartCoroutineEx(healBabyHeal(newData.unit), out newData.controller);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.controller.Stop ();
	}

	private IEnumerator healBabyHeal(Character unit) {
		while (true) {
			unit.heal(healPow);

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
