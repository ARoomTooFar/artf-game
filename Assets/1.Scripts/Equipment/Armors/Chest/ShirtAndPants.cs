using UnityEngine;
using System.Collections;

public class ShirtAndPants : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(1, 1, 10, 10, 50, tier);
	}
}
