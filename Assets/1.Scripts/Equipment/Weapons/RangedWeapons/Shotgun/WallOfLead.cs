using UnityEngine;
using System.Collections;

public class WallOfLead : Shotgun {
	protected override void setInitValues() {
		base.setInitValues();

		this.stats.damage = 15;
		this.stats.goldVal = 150;
	}
}