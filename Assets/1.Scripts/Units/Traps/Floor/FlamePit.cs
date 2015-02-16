using UnityEngine;
using System.Collections;

public class FlamePit : Traps {

	// protected delegate void FireDelegate(Character enemy);

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected override void setInitValues() {
		base.setInitValues ();

		damage = 1;
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per framea
	protected override void Update () {
		base.Update ();
	}

	protected virtual void inFire(Character enemy) {
		if (enemy && enemy.collider.bounds.Intersects(collider.bounds)) {
			enemy.damage (damage);
			StartCoroutine(fireTiming(enemy, 0.3f));
		}
	}

	protected IEnumerator fireTiming(Character enemy, float duration) {
		while (duration > 0) {
			duration -= Time.deltaTime;
			yield return null;
		}
		inFire(enemy);
	}

	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if (component != null && enemy != null) {
			enemy.damage (damage);
			StartCoroutine(fireTiming(enemy, 0.3f));
		}
	}
}
