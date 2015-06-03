// Dagger class, put into the head for now

using UnityEngine;
using System.Collections;

public class PruningBlade : Dagger {
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		
		stats.damage = (int)(20);
		stats.goldVal = 200;
	}
	
}
