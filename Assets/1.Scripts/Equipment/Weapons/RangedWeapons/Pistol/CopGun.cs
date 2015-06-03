using UnityEngine;
using System.Collections;

public class CopGun : Pistol {

	protected override void setInitValues() {
		base.setInitValues();

		this.stats.damage = 15;
		this.stats.goldVal = 150;
	}
}
