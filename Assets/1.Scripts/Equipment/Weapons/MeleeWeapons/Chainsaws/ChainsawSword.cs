// Chainsaw class, put into the head for now, could possibly expand this into a special type weapon os similarity to flamethrower

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainsawSword : Chainsaw {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.goldVal = 80;
		stats.damage = 8;
	}
}
