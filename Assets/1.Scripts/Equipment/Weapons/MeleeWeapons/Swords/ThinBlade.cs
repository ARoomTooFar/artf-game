using UnityEngine;
using System.Collections;

public class ThinBlade : Sword {

	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();

		stats.damage = 33;//  + user.GetComponent<Character>().stats.strength;
		stats.goldVal = 330;
	}
}
