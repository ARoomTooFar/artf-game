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

	public void setSpeedAmpTimes(float amp, float duration) {
		spdPercents.setAmplification(amp);
		subMono.StartCoroutine(ampSpeed(amp, duration));
	}

	public void setSpeedRedTimes(float red, float duration) {
		spdPercents.setReduction(red);
		subMono.StartCoroutine(redSpeed(red, duration));
	}

	private IEnumerator redSpeed(float red, float duration) {
		while (duration > 0) {
			duration -= Time.deltaTime;
			if (duration > 0) {
				yield return null;
			}
		}
		spdPercents.removeReduction(red);
	}

	private IEnumerator ampSpeed(float amp, float duration) {
		while (duration > 0) {
			duration -= Time.deltaTime;
			if (duration > 0) {
				Debug.Log ("Hi");
				yield return null;
			}
		}
		spdPercents.removeAmplification(amp);
	}
}