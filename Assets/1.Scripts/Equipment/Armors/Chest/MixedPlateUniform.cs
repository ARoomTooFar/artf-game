using UnityEngine;
using System.Collections;

public class MixedPlateUniform : Armor {
	protected override void SetInitValues() {
		base.SetInitValues();
		stats = new ArmorStats(8, 1, 18, 12, 180, tier);
	}
}