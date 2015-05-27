using UnityEngine;
using System.Collections;

public class HiCompressionPistol : Pistol {

	protected override void setInitValues() {
		base.setInitValues();

		this.stats.damage = 25 + user.GetComponent<Character>().stats.coordination;
		this.stats.goldVal = 250;
	}
}
