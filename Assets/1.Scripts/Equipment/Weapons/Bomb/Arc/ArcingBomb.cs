using UnityEngine;
using System.Collections;
using System;

public class ArcingBomb : Bomb {	

	protected float speed = 15.0f;

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
		this.shoot (this.calculateTrajectory (targetLocation.transform.position));
		StartCoroutine (reachedDestination (targetLocation));
	}

	//-------------------//


	//---------------------//
	// Protected Functions //
	//---------------------//

	protected virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}

	//---------------------//
	

	//-----------------------//
	// Calculation Functions //
	//-----------------------//

	protected virtual Vector3 calculateTrajectory(Vector3 location) {
		// Calculates distance from source to target. removes height from calculation
		location.y = 0.0f;
		Vector3 temp = this.transform.position;
		temp.y = 0.0f;
		float dist = Vector3.Distance (temp, location);

		// Get dir of z due to angle not accounting for this direction
		float zDir = 0.0f;
		if (location.z - temp.z != 0.0f) {
			zDir = (location.z - temp.z) / Mathf.Abs (location.z - temp.z);
		} 

		// Angle of elevation
		float angle = Mathf.Asin ((9.81f * dist) / (this.speed * this.speed)) / 2.0f;

		// Calculates angle between x and z
		Vector3 direction = location - temp;
		float newAngle = Vector3.Angle (Vector3.right, direction) * Mathf.Deg2Rad;

		// What speed to disburse between x and z after calculating speed for y
		float speedOverLand = Mathf.Sin (angle) * this.speed;
		return new Vector3(speedOverLand * Mathf.Cos (newAngle), this.speed * Mathf.Cos(angle), zDir * speedOverLand * Mathf.Sin (newAngle));
	}

	//-----------------------//


	//-----------//
	// Coroutine //
	//-----------//

	// Removes target circle and stops the bomb from moving so it explodes
	//     May use a second collider on bomb in the future for accuracy and wall collisions
	protected virtual IEnumerator reachedDestination(GameObject destination) {
		while (Vector3.Distance(this.transform.position, destination.transform.position) > 0.5f) {
			yield return null;
		}
		Destroy (destination);
		this.explode ();
	}

	//-----------//

	//-------------//
	// Inheritence //
	//-------------//

	protected override void explode() {
		this.GetComponent<Rigidbody> ().useGravity = false;
		this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		base.explode ();
	}

	//-------------//

}
