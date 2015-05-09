using UnityEngine;
using System.Collections;

public class Pistol : RangedWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();

		stats.weapType = 4;
		stats.weapTypeName = "pistol";
		stats.damage = 10 + user.GetComponent<Character>().stats.coordination;
		stats.maxChgTime = 5;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void AttackStart() {
		this.fireProjectile();
	}

	public override void SpecialAttack() {
		this.fireProjectile();
	}
	
	public override void initAttack() {
		base.initAttack();
	}
}
