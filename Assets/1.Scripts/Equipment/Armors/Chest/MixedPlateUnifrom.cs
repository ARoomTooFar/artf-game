using UnityEngine;
using System.Collections;

public class MixedPlateUniform : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		stats = new ArmorStats(7, 1, 18, 12, 180, tier);
	}
}
