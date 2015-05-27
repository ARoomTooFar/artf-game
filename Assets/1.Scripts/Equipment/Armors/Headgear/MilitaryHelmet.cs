using UnityEngine;
using System.Collections;

public class MilitaryHelmet : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(2, 1, 5, 8, 35, tier);
	}
}