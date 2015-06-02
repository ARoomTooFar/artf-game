using UnityEngine;
using System.Collections;

public class BrainCaseVisor : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(2, 1, 15, 5, 35, tier);
	}
}
