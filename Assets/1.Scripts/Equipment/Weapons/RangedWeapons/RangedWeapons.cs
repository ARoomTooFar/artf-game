// Ranged weapon class
//     For weapons that fire out a projectile

using UnityEngine;
using System.Collections;

public class RangedWeapons : Weapons {

	public GameObject projectile;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected override void setInitValues() {
		base.setInitValues();
	}

	public override void initAttack() {
		base.initAttack();
	}
}