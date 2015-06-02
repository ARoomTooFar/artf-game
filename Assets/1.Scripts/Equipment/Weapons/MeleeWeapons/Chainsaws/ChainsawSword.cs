// Chainsaw class, put into the head for now, could possibly expand this into a special type weapon os similarity to flamethrower

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainsawSword : Chainsaw {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		// User dagger vars for now until we have chainsaw animations
		stats.weapType = 3;
		stats.goldVal = 80;
		stats.atkSpeed = 3.0f;
		stats.damage = 8;
	}
}
