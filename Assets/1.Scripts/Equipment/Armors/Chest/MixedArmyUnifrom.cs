using UnityEngine;
using System.Collections;

public class MixedArmyUniform : Chest {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(6, 1, 12, 20, 180, tier);
	}
}
