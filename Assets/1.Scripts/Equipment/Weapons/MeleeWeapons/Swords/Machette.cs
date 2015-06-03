using UnityEngine;
using System.Collections;

public class Machette : Sword {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.damage = 23;
		stats.goldVal = 230;
	}
}
