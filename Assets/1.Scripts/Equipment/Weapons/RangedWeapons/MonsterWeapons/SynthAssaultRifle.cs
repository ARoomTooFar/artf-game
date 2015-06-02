using UnityEngine;
using System.Collections;

public class SynthAssaultRifle : AssaultRifle {
	
	protected override void setInitValues() {
		base.setInitValues();
		
		this.stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		this.stats.maxChgTime = 5;
		
		this.spread = 10;
		this.maxSpread = 60;
		this.baseSpread = spread;
	}
}
