/*using UnityEngine;
using System.Collections;

public class SpeedBD : Stacking {

	float percent;

	public SpeedBD () {
		name = "SpeedBD";
	}
	
	public SpeedBD (float value){
		name = "SpeedBD";
		percent = value;
	}
	
	protected override void bdEffects(BDData newData){
		base.bdEffects (newData);
		newData.unit.stats.spdManip.setSpeedAmplification(percent);
	}

	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.spdManip.removeSpeedAmplification(percent);
	}
	
	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}

public class StrBD : Stacking {
	
	int percent;
	
	public StrBD () {
		name = "StrBD";
	}
	
	public StrBD (float value){
		name = "StrBD";
		percent = value;
	}
	
	protected override void bdEffects(BDData newData){
		base.bdEffects (newData);
		newData.unit.stats.dmgManip.setDamageAmplification (percent);
	}

	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.dmgManip.removeDamageAmplification (percent);
	}
	
	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}

public class DefBD : Stacking {
	
	int percent;
	
	public DefBD () {
		name = "DefBD";
	}
	
	public DefBD (float value){
		name = "DefBD";
		percent = value;
	}
	
	protected override void bdEffects(BDData newData){
		base.bdEffects (newData);
		newData.unit.stats.dmgManip.setDamageReduction (percent);
	}

	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		oldData.unit.stats.dmgManip.removeDamageReduction (percent);
	}
	
	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
	}
}

public class StatsBD : MonoBehaviour {
	

}*/
