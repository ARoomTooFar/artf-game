using UnityEngine;
using System.Collections;

public class SmugglersJacket : Armor {

	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(3, 1, 12, 14, 120, tier);
	}
}
