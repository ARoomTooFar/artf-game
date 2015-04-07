using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallDarts : Traps {

	// In seconds
	public int dartInterval;

	private Knockback debuff;

	protected float timeSinceLastFire;

	// protected List<Character> unitsInTrap;
	protected AoETargetting aoe;
	protected ParticleSystem darts;
	protected bool firing;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		darts = GetComponent<ParticleSystem> ();
		aoe = GetComponent<AoETargetting>();
		aoe.affectEnemies = true;
		aoe.affectPlayers = true;
		// unitsInTrap = new List<Character>();
		firing = true;
		debuff = new Knockback();
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

			if (this.gameObject.activeSelf) {
				StartCoroutine(countDown());
			}
		}
	}

	protected virtual IEnumerator countDown() {
		while (timeSinceLastFire < dartInterval) {
			timeSinceLastFire += Time.deltaTime;
			yield return null;
		}
		firing = true;
		if (aoe.unitsInRange.Count > 0) fireDarts ();
	}


	public void unitEntered(Character entered) {
		this.fireDarts();
	}

	/*
	void OnTriggerEnter(Collider other) {
		Character enemy = other.GetComponent<Character>();
		
		if (enemy != null) {
			unitsInTrap.Add(enemy);
			fireDarts ();
		}
	}

	void OnTriggerExit(Collider other) {
		Character enemy = other.GetComponent<Character>();
		
		if (enemy != null) {
			if (unitsInTrap.Contains(enemy)) {
				unitsInTrap.Remove (enemy);
			}
		}
	}*/

	void OnParticleCollision(GameObject other) {
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		IForcible<Vector3, float> component2 = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(damage);

			if (component2 != null) {
				enemy.BDS.addBuffDebuff(debuff, this.transform.parent.gameObject, .5f);
			}
		}
	}

	void OnEnable() {
		if (aoe.unitsInRange != null) {
			aoe.unitsInRange.Clear();
		}
		firing = true;
	}
}
