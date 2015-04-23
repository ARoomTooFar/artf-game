using UnityEngine;
using System.Collections;

public class MonsterData : LevelEntityData {
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
}
