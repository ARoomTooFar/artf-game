using UnityEngine;
using System.Collections;

public class LaserRifle : Rifle {

	protected override void setInitValues() {
		base.setInitValues();
		
		stats.damage = 40;
		stats.goldVal = 400;
	}
}