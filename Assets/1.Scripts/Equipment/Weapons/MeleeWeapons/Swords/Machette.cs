using UnityEngine;
using System.Collections;

public class Machette : Sword {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.chargeSlow = new Slow(0.0f);

		stats.damage = 23;//  + user.GetComponent<Character>().stats.strength;
		stats.goldVal = 230;
	}
}
