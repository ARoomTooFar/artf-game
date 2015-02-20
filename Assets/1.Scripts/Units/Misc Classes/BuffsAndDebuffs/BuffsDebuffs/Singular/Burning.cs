// Once hit with fire (Fire pit, flamethrower, etc) unit will be burning
//     Small DOT on unit for a short time, 

using UnityEngine;
using System.Collections;

public class Burning : Singular {

	private int dmg;
	private float dur;

	public Burning(int damage, float duration) {
		name = "Burn";
		dmg = damage;
		dur = duration;
	}
	
	public override void applyBD(Character unit) {
		base.applyBD(unit);
		unit.GetComponent<MonoBehaviour>().StartCoroutine(burnBabyBurn(unit, dur));
	}
	
	public override void removeBD(Character unit) {
		base.removeBD(unit);
	}
	
	private IEnumerator burnBabyBurn(Character unit, float duration) {
		while (duration > 0.0f) {
			duration -= 1.0f;
			yield return new WaitForSeconds(1.0f);
			unit.damage(dmg);
		}
	}

	public override void purgeBD(Character unit) {
		base.purgeBD (unit);
	}
}