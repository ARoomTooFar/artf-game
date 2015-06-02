using UnityEngine;
using System.Collections;

public class CeramicPlate : Armor {
	// Used for setting stats for each weapon piece
	protected override void SetInitValues() {
		base.SetInitValues();
		this.stats = new ArmorStats(8, 1, 20, 22, 200, tier);
	}
}
