using UnityEngine;
using System.Collections;

public class CyberFaceHorns : Helmet {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(4, 1, 8, 8, 35, tier);
	}
}