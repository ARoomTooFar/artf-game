// Percent Value class
//     Used for values that are going to be percent based and stack (Slows, damage reduction/amplification)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PercentValues {
	public float percentValue {get; protected set;}

	// Our list of percent reductions
	//     - Consider using trees instead
	//     - Also consider seperating reduction and amplfications into separate lists
	private List<float> pntChanges;

	public PercentValues() {
		percentValue = 1.0f;
		pntChanges = new List<float>();
	}

	// Setting and Removing percent reductions from the list
	public void setReduction(float redValue) {
		if (redValue > 1.0f) {
			Debug.LogWarning("Attempting to set a reduction greater than 1.0f. Setting reduction value of " + redValue + " to 1.0f.");
			pntChanges.Add (-1.0f);
		} else {
			pntChanges.Add (-redValue);
		}
		calculatePercent();
	}
	
	public void removeReduction(float redValue) {
		if (pntChanges.Contains (-redValue)) {
			pntChanges.Remove (-redValue);
			calculatePercent ();
		} else {
			Debug.LogWarning("Reduction " + -redValue + " does not exist.");
		}
	}

	public void setAmplification(float ampValue) {
		pntChanges.Add (ampValue);
		calculatePercent();
	}

	public void removeAmplification(float ampValue) {
		if (pntChanges.Contains (ampValue)) {
			pntChanges.Remove (ampValue);
			calculatePercent ();
		} else {
			Debug.LogWarning("Reduction " + ampValue + " does not exist.");
		}
		pntChanges.Remove (ampValue);
		calculatePercent();
	}

	// Calculates the total percent after every addition or removal
	private void calculatePercent() {
		float tempValue = 1.0f;
		foreach (float percent in pntChanges) {
			tempValue += tempValue * percent;
		}
		percentValue = tempValue;
	}
}

