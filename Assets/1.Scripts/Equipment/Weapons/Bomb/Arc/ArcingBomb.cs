using UnityEngine;
using System.Collections;
using System;

public class ArcingBomb : Bomb {	

	protected float speed = 20.0f;

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
	}

	//-------------------//


	//---------------------//
	// Protected Functions //
	//---------------------//

	protected virtual void shoot(Vector3 trajectory) {

		print (trajectory);
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}

	//---------------------//



	//-----------------------//
	// Calculation Functions //
	//-----------------------//

	protected virtual Vector3 calculateTrajectory(Vector3 location) {
		location.y = 0.0f;
		Vector3 temp = this.transform.position;
		temp.y = 0.0f;
		float dist = Vector3.Distance (temp, location);

		float xDir = 0.0f;
		if (location.x - temp.x != 0.0f) {
			xDir = (location.x - temp.x) / Mathf.Abs (location.x - temp.x);
		}

		float zDir = 0.0f;
		if (location.z - temp.z != 0.0f) {
			zDir = (location.z - temp.z) / Mathf.Abs (location.z - temp.z);
		} 
		
		float angle = Mathf.Asin ((9.81f * dist) / (this.speed * this.speed)) / 2.0f;


		Vector3 direction = location - temp;
		direction.x = Mathf.Abs (direction.x);
		direction.z = Mathf.Abs (direction.z);

		float newAngle = Vector3.Angle (Vector3.forward, direction);

		/*
		float rotateTrajectoryBy = Vector3.Angle (Vector3.left, temp - location);


		Vector3 trajectory = new Vector3(xDir * this.speed * Mathf.Sin (angle), this.speed * Mathf.Cos(angle), 0.0f);
		Quaternion qRotation = Quaternion.AngleAxis (rotateTrajectoryBy, Vector3.up);
		trajectory = qRotation * trajectory;
		print (trajectory);

		return trajectory;

		*/

		float secondary = Mathf.Sin (angle) * this.speed;

		// print (Mathf.Sin (secondary) + ", " + Mathf.Cos (secondary));

		// return new Vector3(xDir * Mathf.Sin (secondary), Mathf.Cos(angle), zDir * Mathf.Asin (secondary));
		
		// return new Vector3(xDir * this.speed * Mathf.Sin (secondary), this.speed * Mathf.Cos(angle), zDir * this.speed * Mathf.Acos (secondary));
		return new Vector3(xDir * secondary * Mathf.Sin (newAngle), this.speed * Mathf.Cos(angle), zDir * secondary * Mathf.Cos (newAngle));
	}

	//-----------------------//
}
