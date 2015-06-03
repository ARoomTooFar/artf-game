// Spear class, put into the head for now

using UnityEngine;
using System.Collections;

public class FlamePike: Spear {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.goldVal = 400;
		stats.damage = 40;
	}
}
