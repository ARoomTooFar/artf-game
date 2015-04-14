// Bomb parent class
//     For things that do a disjointed explosion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FodderBall: MonoBehaviour {
	
	// Units to be spawned
	public FoliantHive hive;
	public GameObject foliantFodder;
	public AoETargetting aoe;

	//Protected variables
	protected float speed = 15.0f;
	protected GameObject targetLocation;
	
	
	//-------------------//
	// Primary Functions //
	//-------------------//
	
	// Use this for initialization
	protected virtual void Start() {
		if (foliantFodder == null) Debug.LogWarning ("No unit to spawn");
	}
	
	// Update is called once per frame
	protected virtual void Update() {
	}
	
	//-------------------------//

	//------------------//
	// Public Functions //
	//------------------//

	public void setTarget(GameObject loc){
		targetLocation = loc;
		this.shoot (loc.transform.position);
	}

	//-------------------------//

	//-----------------------//
	// Protected Functions //
	//-----------------------//

	protected virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}

	//-------------------------//
	
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
	
	
	//---------------------//
	// Explosion Functions //
	//---------------------//
	
	protected virtual void spawn() {
		// Create explosion while removing self
		// BombExplosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<BombExplosion>();
		FoliantFodder newFodder = (Instantiate(foliantFodder, transform.position, transform.rotation) as GameObject).GetComponent<FoliantFodder>();
		//Sets fodder to be in hiveMind mode
		newFodder.setHive (hive);
		hive.addFodder (newFodder);

		Destroy (this.gameObject);
	}

	protected virtual void reachedDestination() {
		Destroy (this.targetLocation);
		this.targetLocation = null;
		this.spawn ();
	}

	
	//----------------------//
	
	
	//----------//
	// Triggers //
	//----------//

	void OnTriggerEnter(Collider other) {
		if (this.targetLocation != null && other.gameObject == this.targetLocation) {
			this.reachedDestination();
		}
	}
	
	//--------------//
}
