using UnityEngine;
using System.Collections;

public class BikerHelmet : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(1, 1, 5, 5, 20, tier);
	}
}
