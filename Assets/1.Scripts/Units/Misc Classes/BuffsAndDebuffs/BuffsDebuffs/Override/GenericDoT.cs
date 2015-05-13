using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericDoT : Override {

	private int dmg;

	public GenericDoT(int damage) {
		name = "GeneticDoT";
		dmg = damage;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.StartCoroutineEx(ApplyDoT(newData.unit), out newData.controller);
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.controller.Stop ();
	}
	
	private IEnumerator ApplyDoT(Character unit) {
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
