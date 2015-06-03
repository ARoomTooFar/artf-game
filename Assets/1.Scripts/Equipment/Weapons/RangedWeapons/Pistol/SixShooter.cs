using UnityEngine;
using System.Collections;

public class SixShooter : Pistol {
	protected override void setInitValues() {
		base.setInitValues();

		this.stats.damage = 10;
		this.stats.goldVal = 100;
	}
}