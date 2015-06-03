using UnityEngine;
using System.Collections;

public class HuntingRifle : Rifle {
	protected override void setInitValues() {
		base.setInitValues();
		
		stats.damage = 20;
		stats.goldVal = 200;
	}
}