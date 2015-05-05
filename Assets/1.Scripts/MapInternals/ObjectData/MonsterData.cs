using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterData : LevelEntityData {
	public MonsterData() : base(){
		Tier = 0;
	}

	// The tier of the monster
	public int Tier {get; set;}

	public int Points {
		get;
		private set;
	}
	
	// Base cost of monster which is multiplied by the rank of monster+1

	public int Cost {
		get {return baseCost * (this.Tier + 1);} // Test formula for now
	}
	
	// Return total cost of unit so far (For reselling purposes and possibly level difficulty calculations)
	public int TotalValue {
		get {
			int total = 0;
			for (int t = this.Tier; t >= 0; t--) total = total + (baseCost * (t + 1)); // Based on our test formula ^
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
