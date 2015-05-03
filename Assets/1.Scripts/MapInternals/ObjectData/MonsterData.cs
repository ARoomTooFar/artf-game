using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterData : LevelEntityData {
	public MonsterData() : base(){
		tier = 0;
	}

	// The tier of the monster
	public int tier {get; set;}
	
	// Base cost of monster which is multiplied by the rank of monster+1

	public int cost {
		get {return baseCost * (this.tier + 1);} // Test formula for now
	}
	
	// Return total cost of unit so far (For reselling purposes and possibly level difficulty calculations)
	public int totalValue {
		get {
			int total = 0;
			for (int t = this.tier; t >= 0; t--) total = total + (baseCost * (t + 1)); // Based on our test formula ^
			return total;
		}
	}

	public List<Vector3> RadiusCoordinates(DIRECTION dir) {
		List<Vector3> retVal = new List<Vector3>();
		foreach(Vector3 vec in Coordinates) {
			retVal.Add(vec.RotateTo(dir));
		}
		return retVal;
	}
}
