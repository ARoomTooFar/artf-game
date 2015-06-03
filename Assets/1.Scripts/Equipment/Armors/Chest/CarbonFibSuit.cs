using UnityEngine;
using System.Collections;

public class CarbonFibSuit : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(10, 1, 30, 18, 250, tier);
	}
}
