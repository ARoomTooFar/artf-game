// DamageManipulation class
//     Used for manipulating damage value by a percent before armor 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DamageManipulation {

	public PercentValues genDamage;
	public PercentValues fntDamage;
	public PercentValues backDamage;
	public PercentValues sideDamage;

	public DamageManipulation() {
		genDamage = new PercentValues();
		fntDamage = new PercentValues();
		backDamage = new PercentValues();
		sideDamage = new PercentValues();
	}

	// Gets the total % of the damage change
	// Takes in the facing and position of the unit receiving the damage and the position of the attack(er)
	public float getDmgValue(Vector3 unitFacing, Vector3 unitPos, Vector3 atkPos) {
		float ttlReduction = genDamage.percentValue;

		float angle = Vector2.Angle(new Vector2(unitPos.x - atkPos.x, unitPos.z - atkPos.z), new Vector2(unitFacing.x, unitFacing.z));

		if (angle < 45.0f) {
			ttlReduction *= backDamage.percentValue;
			Debug.Log("Back");
		} else if (angle < 135.0f) {
			ttlReduction *= sideDamage.percentValue;
			Debug.Log("Side");
		} else if (angle <= 180.0f) {
			ttlReduction *= fntDamage.percentValue;
			Debug.Log("Front");
		} else {
			Debug.Log ("I shouldn't be here");
		}
		
		return ttlReduction;
	}
}
