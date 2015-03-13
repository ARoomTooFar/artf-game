using UnityEngine;
using System.Collections;
using System;

public class ArtilleryShell : ArcingBomb {	

	//-------------------//
	// Primary Functions //
	//-------------------//

	protected override void Start() {
		base.Start();
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}

	//--------------------//


	//---------------------//
	// Inherited Functions //
	//---------------------//
	
	public override void setInitValues(Character player, Type opposition, int damage, bool effect, BuffsDebuffs hinder, GameObject targetLocation) {
		base.setInitValues(player, opposition, damage, effect, hinder, targetLocation);
	}
	
	//----------------------//
}
