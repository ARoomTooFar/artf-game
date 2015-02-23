// Once hit with fire (Fire pit, flamethrower, etc) unit will be burning
//     Small DOT on unit for a short time, 

using UnityEngine;
using System.Collections;

public class Stun : Override {
	
	public Stun() {
		name = "Stun";
	}

	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
		unit.removeStun();
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);
		newData.unit.stun();
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
	}
}