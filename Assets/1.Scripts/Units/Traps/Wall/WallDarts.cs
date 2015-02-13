using UnityEngine;
using System.Collections;

public class WallDarts : Traps {

	// In seconds
	public int dartInterval;
	protected float timeSinceLastFire;

	protected int unitsInTrap;
	protected ParticleSystem darts;
	protected bool firing = true;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		darts = GetComponent<ParticleSystem> ();
		unitsInTrap = 0;
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

	protected virtual void fireDarts() {
		if (firing) {
			darts.Emit (50);
			firing = false;
			timeSinceLastFire = 0.0f;
			StartCoroutine(countDown());
		}
	}

	protected virtual IEnumerator countDown() {
		while (timeSinceLastFire < dartInterval) {
			timeSinceLastFire += Time.deltaTime;
			yield return null;
		}
		firing = true;
		if (unitsInTrap > 0) fireDarts ();
	}

	void OnTriggerEnter(Collider other) {
		unitsInTrap++;
		fireDarts ();
	}

	void OnTriggerExit(Collider other) {
		unitsInTrap--;
	}

	void OnParticleCollision(GameObject other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(damage);
		}
	}
}
