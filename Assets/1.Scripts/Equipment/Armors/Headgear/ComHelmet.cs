using UnityEngine;
using System.Collections;

public class ComHelmet : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(1, 1, 5, 3, 25, tier);
	}
}
