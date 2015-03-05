using UnityEngine;
using System.Collections;

public class Frenzy : Override {

	private float spd;
	private float str;
	private float def;
	float threshold;
	int level;

	public Frenzy () {
		level = 1;
		name = "Frenzy";
		spd = 100;
		str = 100;
		def = 100;
	}

	public void setSpeedBoost(float percent){
		spd = percent;
	}

	public void setStrBoost(float percent){
		str = percent;
	}

	public void setDefBoost(float percent){
		def = percent;
	}

	public void FrenzyUp(){

	}

	public void reset(){

	}

	protected override void bdEffects(BDData newData){
		base.bdEffects (newData);
		newData.unit.stats.dmgManip.setDamageReduction(0, def);
		newData.unit.stats.spdManip.setSpeedAmplification(spd);
		newData.unit.stats.dmgManip.setDamageAmplification(0, str);
	}

	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.dmgManip.removeDamageReduction(0, def);
		oldData.unit.stats.spdManip.removeSpeedAmplification(spd);
		oldData.unit.stats.dmgManip.removeDamageAmplification(0, str);
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}
