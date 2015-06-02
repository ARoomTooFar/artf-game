using UnityEngine;
using System.Collections;

public class AutomaticAssaultRifle : AssaultRifle {
	protected override void setInitValues() {
		base.setInitValues();
		
		this.stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		this.stats.goldVal = 100;
	}
}