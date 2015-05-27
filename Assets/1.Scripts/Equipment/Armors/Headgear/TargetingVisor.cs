using UnityEngine;
using System.Collections;

public class TargetingVisor : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(2, 1, 10, 8, 35, tier);
	}
}