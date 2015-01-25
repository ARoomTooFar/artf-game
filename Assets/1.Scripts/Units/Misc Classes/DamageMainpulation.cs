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
	List<float> genChanges = new List<float>();
	List<float> fntChanges = new List<float>();
	List<float> backChanges = new List<float>();
	List<float> sideChanges = new List<float>();
	
	// Gets the total % of the damage change
	// Takes in the facing and position of the unit receiving the damage and the position of the attack(er)
	public float getDmgValue(Vector3 unitFacing, Transform unitPos, Vector3 atkPos) {
		float ttlReduction = genValue;
		
		Vector3 distance = atkPos - unitPos.position;
		Vector3 relativePosition = Vector3.zero;
		
		relativePosition.x = Vector3.Dot (distance, unitPos.right.normalized);
		relativePosition.y = 0.0f;
		relativePosition.z = Vector3.Dot (distance, unitPos.forward.normalized);
		
		relativePosition = relativePosition.normalized;
		unitFacing = unitFacing.normalized;
	
	
	//	Vector3 newFacing = new Vector3(unitFacing.x * Mathf.Cos(135) + unitFacing.z * Mathf.Sin(135), 0.0f, -unitFacing.x * Mathf.Sin(135) + unitFacing.z * Mathf.Cos(135));
		Debug.Log(getVectorByDegree(unitFacing, 135));
		Debug.Log(getVectorByDegree(unitFacing, -135));
		
		// if (relativePosition.x < unitFacing.x)
		
		
		
		return ttlReduction;
	}
	
	// Gets the vector of a vector rotated by some degree
	private Vector3 getVectorByDegree(Vector3 vec, float rotDegree) {
		return new Vector3(vec.x * Mathf.Cos(rotDegree) + vec.z * Mathf.Sin(rotDegree),
				0.0f,
				-vec.x * Mathf.Sin(rotDegree) + vec.z * Mathf.Cos(rotDegree));
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
