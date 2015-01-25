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

	protected override void FixedUpdate() {
		base.FixedUpdate();

		// Placed here for consistent damage
		if (Time.time - lastDmgTime >= 0.1f) {
			dealDamage = true;
			lastDmgTime = Time.time;
		} else {
			dealDamage = false;
		}
	}

	// Update is called once per framea
	protected override void Update () {
		base.Update ();
	}

	void OnTriggerStay(Collider other) {
		IDamageable<int, Vector3> component = (IDamageable<int, Vector3>) other.GetComponent( typeof(IDamageable<int, Vector3>) );
		if (dealDamage && component != null) {
			component.damage (stats.damage);
		}
	}
}
