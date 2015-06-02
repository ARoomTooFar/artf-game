using UnityEngine;
using System.Collections;

public class TrashHelmetLB : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(0, 1, 2, 2, 10, tier);
	}
}