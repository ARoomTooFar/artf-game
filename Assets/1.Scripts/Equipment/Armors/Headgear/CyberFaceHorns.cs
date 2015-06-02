using UnityEngine;
using System.Collections;

public class CyberFaceHorns : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(4, 1, 8, 8, 35, tier);
	}
}