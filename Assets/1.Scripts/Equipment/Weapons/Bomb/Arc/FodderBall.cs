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
	protected float angle = 25.0f;
	protected GameObject targetLocation;
	protected bool hitWall = false;
	
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
		this.shoot (this.transform.AngledArcTrajectory(targetLocation.transform.position, angle));
	}

	//-------------------------//

	//-----------------------//
	// Protected Functions //
	//-----------------------//

	protected virtual void shoot(Vector3 trajectory) {
		this.GetComponent<Rigidbody> ().velocity = trajectory;
	}

	//-------------------------//
	
	
	//---------------------//
	// Explosion Functions //
	//---------------------//
	
	protected virtual void spawn() {
		// Create explosion while removing self
		// BombExplosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<BombExplosion>();
		Vector3 fodderPos = this.transform.position;
		fodderPos.y = 0.0f;
		FoliantFodder newFodder = (Instantiate(foliantFodder, fodderPos, transform.rotation) as GameObject).GetComponent<FoliantFodder>();
		//Sets fodder to be in hiveMind mode
		newFodder.SetMonster(1);
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
