using UnityEngine;
using System.Collections;

public class Bullet : Projectile {	
	// Use this for initialization
	protected override void Start() {
		base.Start();
	}

	public override void setInitValues(Character player, float partSpeed) {
		base.setInitValues(player, partSpeed);

		// Set stats here is each bullet will have its own properties
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}
}
