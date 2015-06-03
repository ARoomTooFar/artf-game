using UnityEngine;
using System.Collections;

public class AutomaticAssaultRifle : AssaultRifle {
	protected override void setInitValues() {
		base.setInitValues();
		
		this.stats.damage = 10;
		this.stats.goldVal = 100;
	}
}