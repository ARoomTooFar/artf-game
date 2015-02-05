using UnityEngine;
using System.Collections;

public class Bullet : Projectile {	
	// Use this for initialization
	protected override void Start() {
		base.Start();
	}
	protected override void setInitValues() {
		base.setInitValues();
		//damage = 1;
		//speed = .35f;
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}
}
