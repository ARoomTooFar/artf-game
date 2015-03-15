using UnityEngine;
using System.Collections;

public class Immolation : FlamePit{
	public float timing;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected virtual void setInitValues(float starter) {
		base.setInitValues ();
		timing = starter;
		//damage = 1;
		debuff = new Burning ((int)starter);
		StartCoroutine(burnOut(timing));
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	protected override void inFire(Character enemy) {
		if (enemy && enemy.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds)) {
			enemy.BDS.addBuffDebuff(debuff, this.gameObject, timing);
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
	protected IEnumerator burnOut(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			//testable = true;
			yield return 0;
		}
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if (component != null && enemy != null) {
			enemy.BDS.addBuffDebuff(debuff, this.gameObject, timing);
			StartCoroutine(fireTiming(enemy, 0.3f));
		}
	}
}
