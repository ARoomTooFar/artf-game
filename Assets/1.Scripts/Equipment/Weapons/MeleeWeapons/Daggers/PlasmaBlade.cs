// Dagger class, put into the head for now

using UnityEngine;
using System.Collections;

public class PlasmaBlade : Dagger {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		// Default sword stats
		stats.weapType = 1;
		stats.atkSpeed = 2.0f;
		stats.damage = (int)(30);
		this.stats.buffDuration = 0.25f;
		stats.goldVal = 250;
	}
}
