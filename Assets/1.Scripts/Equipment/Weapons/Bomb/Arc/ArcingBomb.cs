using UnityEngine;
using System.Collections;
using System;

public class ArcingBomb : Bomb {	

	protected float angle = 25.0f;
	protected GameObject targetLocation;

	//-------------------//
	// Primary Functions //
	//-------------------//

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();
	}

	//---------------------//


	//------------------//
	// Public Functions //
	//------------------//

	public virtual void setInitValues(Character player, Type opposition, int damage, bool effect, BuffsDebuffs hinder, GameObject targetLocation) {
		base.setInitValues(player, opposition, damage, effect, hinder);
		this.targetLocation = targetLocation;
		this.shoot (this.transform.AngledArcTrajectory(targetLocation.transform.position, angle));
	}

	//-------------------//


	//---------------------//
	// Protected Functions //
	//---------------------//

	protected virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}

	protected virtual void reachedDestination() {
		Destroy (this.targetLocation);
		this.targetLocation = null;
		this.explode ();
	}

	//---------------------//
	

	//-------------//
	// Inheritence //
	//-------------//

	protected override void explode() {
		this.GetComponent<Rigidbody> ().useGravity = false;
		this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		base.explode ();
	}

	//-------------//


	void OnTriggerEnter(Collider other) {
		if (this.targetLocation != null && other.gameObject == this.targetLocation) {
			this.reachedDestination();
		}
	}
}
