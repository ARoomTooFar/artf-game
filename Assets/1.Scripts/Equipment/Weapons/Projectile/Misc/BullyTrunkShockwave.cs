using UnityEngine;
using System.Collections;
using System;

public class BullyTrunkShockwave : EnergyProjectile {

	private Knockback debuff;

	// Use this for initialization
	protected override void Start() {
		base.Start();

		debuff = new Knockback();
	}
	
	public override void setInitValues(Character player, Type opposition, int dmg, bool effect, BuffsDebuffs hinder) {
		this.user = player;
		this.opposition = opposition;

		if (effect) {
			transform.Rotate(Vector3.up * 90);
		} else {
			transform.Rotate(Vector3.up * 270);
		}


		speed = 1.0f;
		lifeTime = 0.1f;
		curLifeTime = 0.0f;
		// Set stats here is each bullet will have its own properties
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}
	
	void OnTriggerEnter(Collider other) {
		IForcible<Vector3, float> component2 = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component2 != null && enemy != null) {
			enemy.BDS.addBuffDebuff(debuff, this.user.gameObject, .5f);
		}
	}
}
