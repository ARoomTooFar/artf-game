using UnityEngine;
using System.Collections;

public class AutomaticLaserRifle : AssaultRifle {

	protected override void setInitValues() {
		base.setInitValues();
		
		this.stats.damage = 15 + user.GetComponent<Character>().stats.coordination;
		this.stats.goldVal = 150;
	}
}
