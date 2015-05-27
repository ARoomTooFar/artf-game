using UnityEngine;
using System.Collections;

public class LaserRifle : Rifle {

	protected override void setInitValues() {
		base.setInitValues();
		
		stats.damage = 40 + user.GetComponent<Character>().stats.coordination;
		stats.goldVal = 400;
	}
}