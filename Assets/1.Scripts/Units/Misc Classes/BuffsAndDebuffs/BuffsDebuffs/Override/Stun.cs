// Once hit with fire (Fire pit, flamethrower, etc) unit will be burning
//     Small DOT on unit for a short time, 

using UnityEngine;
using System.Collections;

public class Stun : Override {
	
	public Stun() {
		name = "Stun";
	}
	
	public override void applyBD(Character unit, GameObject source) {
		base.applyBD(unit, source);
		unit.stun();
	}
	
	public override void removeBD(Character unit, GameObject source) {
		base.removeBD(unit, source);
		unit.removeStun();
	}
	
	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
		unit.removeStun();
	}
}