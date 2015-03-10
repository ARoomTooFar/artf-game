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
		PercentValue = tempValue;
	}
}

public class StatsMultiplier{
	public float health, armor,maxHealth,rezCount;
	public float strength, coordination, speed, luck;

	public int minAtkRadius, maxAtkRadius;
	public float dmgAmp, dmgRed;
}

public class PowerLevels{
	private class PowerStage
	{
		public int powerCount;
		public StatsMultiplier mult;
	}

	List<PowerStage> tiers;
	int powvalue;
	int currentStage;

	public PowerLevels(){
		tiers = new List<PowerStage>();
		powvalue = 0;
		currentStage = -1;
	}

	public void addStage(StatsMultiplier statsbd, int powval){
		if(currentStage == -1) currentStage = 0;
		PowerStage newtier = new PowerStage ();
		newtier.powerCount = powval;
		newtier.mult = statsbd;
		tiers.Add (newtier);
	}

	public StatsMultiplier getStatsBD(){
		return tiers[currentStage].mult;
	}

	public void Update(){
		if (currentStage < tiers.Count){
			if(powvalue > tiers [currentStage].powerCount){
				currentStage++;
			}
		}

		if (powvalue < tiers [currentStage].powerCount && currentStage > 0){
			currentStage--;
		}
	}

	public void updateRage(int gaar){
		powvalue = gaar;
	}

	public void addRage(int gaaar){
		powvalue += gaaar;
	}

	public void resetStage(){
		currentStage = 0;
		powvalue = 0;
	}

}