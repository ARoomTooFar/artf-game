using UnityEngine;
using System.Collections;

public class HuntingRifle : Rifle {
	protected override void setInitValues() {
		base.setInitValues();
		
		stats.damage = 20 + user.GetComponent<Character>().stats.coordination;
		stats.goldVal = 200;
	}
}