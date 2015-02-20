// Once hit with fire (Fire pit, flamethrower, etc) unit will be burning
//     Small DOT on unit for a short time, 

using UnityEngine;
using System.Collections;

public class Burning : Singular {

	public Burning() {
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
		unit.GetComponent<MonoBehaviour>().StartCoroutine(burnBabyBurn());
	}
	
	public override void removeBD(Character unit) {
		base.removeBD(unit);
	}
	
	private IEnumerator burnBabyBurn() {
		while (true) {
			yield return null;
		}
	}

	public override void purgeBD(Character unit) {
		base.purgeBD (unit);
	}
}