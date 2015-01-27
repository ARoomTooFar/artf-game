// DamageManipulation class
//     Used for manipulating damage value by a percent before armor 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DamageManipulation {
	
	private float genValue = 1.0f;
	private float fntValue = 1.0f;
	private float backValue = 1.0f;
	private float sideValue = 1.0f;
	
	// Consider swapping over to trees in the future

	// Lists of damage changes based on positions
	// Can be made more intricate later to have a sepaprate amp and reduction lists and other stuff
	List<float> genChanges = new List<float>();
	List<float> fntChanges = new List<float>();
	List<float> backChanges = new List<float>();
	List<float> sideChanges = new List<float>();
	
	// Gets the total % of the damage change
	// Takes in the facing and position of the unit receiving the damage and the position of the attack(er)
	public float getDmgValue(Vector3 unitFacing, Transform unitPos, Vector3 atkPos) {
		float ttlReduction = genValue;
		
		Debug.Log(unitPos.position);
		
		
		//relativePosition.x = Vector3.Dot (atkPos - unitPos.position, unitPos.right.normalized);
		//relativePosition.y = 0.0f;
		//relativePosition.z = Vector3.Dot (atkPos - unitPos.position, unitPos.forward.normalized);
		
		//relativePosition.Normalize();
	
		float angle = Vector2.Angle(new Vector2(unitPos.position.x - atkPos.x, unitPos.position.z - atkPos.z), new Vector2(unitFacing.x, unitFacing.z));
		
		
		if (angle <= 45.0f) {
			ttlReduction *= fntValue;
			Debug.Log("Front");
		} else if (angle <= 135.0f) {
			ttlReduction *= sideValue;
			Debug.Log("Side");
		} else if (angle <= 180.0f) {
			ttlReduction *= backValue;
			Debug.Log("Back");
		} else {
			Debug.Log ("I shouldn't be here");
		}
		
		return ttlReduction;
	}
	
	// Damage Reduction Functions
	public void setGeneralReduction(float redValue) {
		genChanges.Add (-redValue);
		calculateGeneral();
	}
	
	public void removeGeneralReduction(float redValue) {
		genChanges.Remove (-redValue);
		calculateGeneral ();
	}
	
	public void setFrontReduction(float redValue) {
		fntChanges.Add (-redValue);
		calculateFront();
	}
	
	public void removeFrontReduction(float redValue) {
		fntChanges.Remove (-redValue);
		calculateFront ();
	}
	
	public void setBackReduction(float redValue) {
		backChanges.Add (-redValue);
		calculateBack();
	}
	
	public void removeBackReduction(float redValue) {
		backChanges.Remove (-redValue);
		calculateBack ();
	}
	
	public void setSideReduction(float redValue) {
		sideChanges.Add (-redValue);
		calculateSide ();
	}
	
	public void removeSideReduction(float redValue) {
		sideChanges.Remove (-redValue);
		calculateSide ();
	}
	
	
	// Damage Amplification Functions
	public void setGeneralAmplification(float ampValue) {
		genChanges.Add (ampValue);
		calculateGeneral();
	}
	
	public void removeGeneralAmplification(float ampValue) {
		genChanges.Remove (ampValue);
		calculateGeneral ();
	}
	
	public void setFrontAmplification(float ampValue) {
		fntChanges.Add (ampValue);
		calculateFront();
	}
	
	public void removeFrontAmplification(float ampValue) {
		fntChanges.Remove (ampValue);
		calculateFront ();
	}
	
	public void setBackAmplification(float ampValue) {
		backChanges.Add (ampValue);
		calculateBack();
	}
	
	public void removeBackAmplification(float ampValue) {
		backChanges.Remove (ampValue);
		calculateBack ();
	}
	
	public void setSideAmplification(float ampValue) {
		sideChanges.Add (ampValue);
		calculateSide ();
	}
	
	public void removeSideAmplification(float ampValue) {
		sideChanges.Remove (ampValue);
		calculateSide ();
	}
	
	// After additions/subtractions, values are calculated with these methods
	private void calculateGeneral() {
		float tempValue = 1.0f;
		foreach (float gC in genChanges) {
			tempValue += tempValue * gC;
		}
		genValue = tempValue;
	}
	
	private void calculateFront() {
		float tempValue = 1.0f;
		foreach (float fC in fntChanges) {
			tempValue += tempValue * fC;
		}
		fntValue = tempValue;
	}
	
	private void calculateBack() {
		float tempValue = 1.0f;
		foreach (float bC in backChanges) {
			tempValue += tempValue * bC;
		}
		backValue = tempValue;
	}
	
	private void calculateSide() {
		float tempValue = 1.0f;
		foreach (float sC in sideChanges) {
			tempValue += tempValue * sC;
		}
		sideValue = tempValue;
	}
}
