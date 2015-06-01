using UnityEngine;
using System.Collections;

public class TrashHelmetBucket : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(0, 1, 5, 5, 10, tier);
	}
}