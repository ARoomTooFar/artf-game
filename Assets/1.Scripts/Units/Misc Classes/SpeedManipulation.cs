// Speed Manipulation class
//     Used for percent speed changes; both for duration or persistent 

using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpeedManipulation {

	public float speedPercent {
		get{return spdPercents.percentValue;}
	}
	private PercentValues spdPercents;
	private MonoBehaviour subMono;

	public SpeedManipulation(MonoBehaviour sM) {
		spdPercents = new PercentValues();
		subMono = sM;
	}

	public void setSpeedAmplification(float amp) {
		spdPercents.setAmplification(amp);
	}

	public void removeSpeedAmplification(float amp) {
		spdPercents.removeAmplification(amp);
	}

	public void setSpeedReduction(float red) {
		spdPercents.setReduction(red);
	}
	
	public void removeSpeedReduction(float red) {
		spdPercents.removeReduction(red);
	}
}