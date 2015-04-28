using UnityEngine;
using System.Collections;

public class CableMaw : NewStationaryEnemy {

	//-------------------//
	// Primary Functions //
	//-------------------//

	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 70;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 5;
		stats.coordination = 0;
		stats.speed = 0;
		stats.luck = 0;
		setAnimHash ();
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}

	//----------------------//
}
