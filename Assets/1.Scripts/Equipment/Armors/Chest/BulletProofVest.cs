using UnityEngine;
using System.Collections;

public class BulletProofVest : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(6, 1, 14, 10, 150, tier);
	}
}
