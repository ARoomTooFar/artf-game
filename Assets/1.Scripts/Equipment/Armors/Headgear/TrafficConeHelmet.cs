using UnityEngine;
using System.Collections;

public class TrafficConeHelmet : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(0, 1, 4, 4, 15, tier);
	}
}