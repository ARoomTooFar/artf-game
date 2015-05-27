using UnityEngine;
using System.Collections;

public class PoliceHelmet : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(0, 1, 7, 7, 20, tier);
	}
}