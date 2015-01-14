using UnityEngine;
using System.Collections;

public class FlamePit : Traps {

	private bool dealDamage;
	private float lastDmgTime;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		dealDamage = false;
		lastDmgTime = 0.0f;
	}

	protected override void setInitValues() {
		base.setInitValues ();
		stats.damage = 1;
	}

	// Update is called once per framea
	protected override void Update () {
		base.Update ();

		if (Time.time - lastDmgTime >= 0.1f) {
			dealDamage = true;
			lastDmgTime = Time.time;
		} else {
			dealDamage = false;
		}
	}

	void OnTriggerStay(Collider other) {
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		if (dealDamage && component != null) {
			component.damage (stats.damage);
		}
	}
}
