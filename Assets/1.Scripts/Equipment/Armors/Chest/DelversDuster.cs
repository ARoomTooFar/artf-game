using UnityEngine;
using System.Collections;

public class DelversDuster : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(8, 1, 18, 35, 220, tier);
	}
}
