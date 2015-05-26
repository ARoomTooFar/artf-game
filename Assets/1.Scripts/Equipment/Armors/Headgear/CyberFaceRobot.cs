using UnityEngine;
using System.Collections;

public class CyberFaceRobot : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(2, 1, 5, 15, 35, tier);
	}
}