// DamageManipulation class
//     Used for manipulating damage value by a percent before armor 

using UnityEngine;
using System.Collections;

public class DamageManipulation {

	// Each direction has their own objects
	private PercentValues genDamage;
	private PercentValues fntDamage;
	private PercentValues backDamage;
	private PercentValues sideDamage;

	// Getters for the damage percents
	public float generalDamagePercent {
		get{return genDamage.percentValue;}
	}
	public float frontDamagePercent {
		get{return fntDamage.percentValue;}
	}
	public float backDamagePercent {
		get{return backDamage.percentValue;}
	}
	public float sideDamagePercent {
		get{return sideDamage.percentValue;}
	}

	//private MonoBehaviour subMono;

	public DamageManipulation() {
		genDamage = new PercentValues();
		fntDamage = new PercentValues();
		backDamage = new PercentValues();
		sideDamage = new PercentValues();

		//subMono = sM;
	}

	// Gets the total % of the damage change
	// Takes in the facing and position of the unit receiving the damage and the position of the attack(er)
	public float getDmgValue(Vector3 atkPos, Vector3 unitFacing, Vector3 unitPos) {
		
		float ttlReduction = genDamage.percentValue;

		if (ARTFUtilities.isBehind (atkPos, unitFacing, unitPos)) {
			ttlReduction *= backDamage.percentValue;
			// Debug.Log ("Back Damage");
		} else if (ARTFUtilities.isOnSide (atkPos, unitFacing, unitPos)) {
			ttlReduction *= sideDamage.percentValue;
			// Debug.Log ("Side Damage");
		} else {
			ttlReduction *= fntDamage.percentValue;
			// Debug.Log ("Front Damage");
		}

		/*
		float angle = Vector2.Angle(new Vector2(unitPos.x - atkPos.x, unitPos.z - atkPos.z), new Vector2(unitFacing.x, unitFacing.z));
		
		if (angle < 45.0f) {
			ttlReduction *= backDamage.percentValue;
		} else if (angle < 135.0f) {
			ttlReduction *= sideDamage.percentValue;
		} else if (angle <= 180.0f) {
			ttlReduction *= fntDamage.percentValue;
		} else {
			Debug.Log ("I shouldn't be here");
		}*/
		
		return ttlReduction;
	}

	public void setDamageAmplification(int dir, float amp) {
		switch(dir) {
			case 1:
				fntDamage.setAmplification(amp);
				break;
			case 2:
				backDamage.setAmplification(amp);
				break;
			case 3:
				sideDamage.setAmplification(amp);
				break;
			default:
				genDamage.setAmplification(amp);
				break;
		}
	}
	
	public void removeDamageAmplification(int dir, float amp) {
		switch(dir) {
			case 1:
				fntDamage.removeAmplification(amp);
				break;
			case 2:
				backDamage.removeAmplification(amp);
				break;
			case 3:
				sideDamage.removeAmplification(amp);
				break;
			default:
				genDamage.removeAmplification(amp);
				break;
		}
	}
	
	public void setDamageReduction(int dir, float red) {
		switch(dir) {
			case 1:
				fntDamage.setReduction(red);
				break;
			case 2:
				backDamage.setReduction(red);
				break;
			case 3:
				sideDamage.setReduction(red);
				break;
			default:
				genDamage.setReduction(red);
				break;
		}
	}
	
	public void removeDamageReduction(int dir, float red) {
		switch(dir) {
			case 1:
				fntDamage.removeReduction(red);
				break;
			case 2:
				backDamage.removeReduction(red);
				break;
			case 3:
				sideDamage.removeReduction(red);
				break;
			default:
				genDamage.removeReduction(red);
				break;
		}
	}
}
