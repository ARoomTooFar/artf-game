// Spear class, put into the head for now

using UnityEngine;
using System.Collections;

public class FlamePike: Spear {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.goldVal = 400;
		stats.weapType = 2;
		this.stats.buffDuration = 1.25f;
		stats.damage = 40; // (int)(10 + 1.5f * user.GetComponent<Character>().stats.strength);
	}
}
