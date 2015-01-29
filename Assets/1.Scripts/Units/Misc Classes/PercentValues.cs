// Percent Value class
//     Used for values that are going to be percent based and stack (Slows, damage reduction/amplification)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PercentValues {
	private float PercentValue;
	public float percentValue { // Makes our percent read only
		get {return PercentValue;}
	}

	// Our list of percent reductions
	//     - Consider using trees instead
	//     - Also consider seperating reduction and amplfications into separate lists
	private List<float> pntChanges;

	public PercentValues() {
		PercentValue = 1.0f;
		pntChanges = new List<float>();
	}

	// Setting and Removing percent reductions from the list
	public void setReduction(float redValue) {
		pntChanges.Add (-redValue);
		calculatePercent();
	}
	
	public void removeReduction(float redValue) {
		pntChanges.Remove (-redValue);
		calculatePercent ();
	}

	public void setAmplification(float ampValue) {
		pntChanges.Add (ampValue);
		calculatePercent();
	}

	public void removeAmplification(float ampValue) {
		pntChanges.Remove (ampValue);
		calculatePercent();
	}

	// Calculates the total percent after every addition or removal
	private void calculatePercent() {
		float tempValue = 1.0f;
		foreach (float percent in pntChanges) {
			tempValue += tempValue * percent;
		}
		PercentValue = tempValue;
	}
}