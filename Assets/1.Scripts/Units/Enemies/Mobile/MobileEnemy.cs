// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobileEnemy : Enemy {

	public CoroutineController searchController, searchStateController;
	
	//-------------------//
	// Primary Functions //
	//-------------------//
	
	// Get players, navmesh and all colliders
	protected override void Awake () {
		base.Awake ();
		resetpos = transform.position;
	}
	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();

		if (this.target != null && this.target.GetComponent<Player>() != null) {
			this.targetDir = this.target.GetComponent<Player>().facing;
		}


		if (!this.lastSeenPosition.HasValue || this.lastSeenSet <= 0.0f) {
			this.animator.SetBool ("HasLastSeenPosition", false);
			this.lastSeenPosition = null;
		} else {
			this.lastSeenSet -= Time.deltaTime;
		}
	}
	
	//----------------------//
	
	
	//------------------//
	// Action Functions //
	//------------------//
	
	public virtual void StopSearch() {
		this.searchController.Stop ();
		this.searchStateController.Stop ();
	}

	
	//------------------//
	
	
	//----------------//
	// Avoidance Code //
	//----------------//
	
	public override void MoveForward(float effectivness = 1f) {
		Vector3 generalDir = Vector3.zero;
		foreach (Collider cols in this.flockDar.objectsInRange) {
			if (cols.gameObject == null) continue;
			Vector3 point = cols.bounds.ClosestPoint(this.transform.position);
			point.y = 0f;
			// Vector3 pointOfContact = this.col.bounds.ClosestPoint(point);
			// pointOfContact.y = 0f;
			Vector3 dis = point - this.transform.position;
			if (Vector3.Angle(dis, this.facing) < 90 || Vector3.Distance(point,this.col.bounds.ClosestPoint(point)) < 0.75f) {
				generalDir += dis;
			}
		}
		generalDir /= flockDar.objectsInRange.Count;
		generalDir *= -1;
		generalDir.Normalize();
		
		//generalDir = Vector3.Slerp (generalDir, this.facing, 0.25f);
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.15f * effectivness);
		transform.localRotation = Quaternion.LookRotation(facing);
		this.rb.velocity = this.facing.normalized * this.stats.speed * this.stats.spdManip.speedPercent;
	}
	
	//----------------//
	
	
	//------------//
	// Coroutines //
	//------------//

	protected IEnumerator moveToPosition(Vector3 position) {
		float moveToTime = 2.0f;
		while ((Vector3.Distance(this.transform.position, this.targetDir) > 0.1f && this.target == null && moveToTime > 0.0f)) {
			// this.rb.velocity = this.facing.normalized * this.stats.speed * this.stats.spdManip.speedPercent;
			this.MoveForward(0.2f);
			moveToTime -= Time.deltaTime;
			yield return null;
		}
	}
	
	protected IEnumerator moveToExpectedArea() {
		// this.facing = this.targetDir;
		float moveToTime = 1.0f;
		while (this.target == null && moveToTime > 0.0f) {
			this.facing = Vector3.Slerp (this.facing, this.targetDir.normalized, 0.25f);
			this.MoveForward();
			moveToTime -= Time.deltaTime;
			yield return null;
		}
	}
	
	protected IEnumerator randomSearch() {;
		float resetTimer = aggroTimer;
		while(resetTimer > 0.0f && this.target == null) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f));
			this.MoveForward();
			yield return new WaitForSeconds (0.5f);
			resetTimer -= 0.5f;
		}
	}
	
	public virtual IEnumerator searchForEnemy(Vector3 lsp) {
		yield return this.StartCoroutineEx(moveToPosition(lsp), out this.searchStateController);
		yield return this.StartCoroutineEx(moveToExpectedArea(), out this.searchStateController);
		yield return this.StartCoroutineEx(randomSearch(), out this.searchStateController);
		
		if (this.target == null) {
			alerted = false;
			this.animator.SetBool("Alerted", false);
		}
	}
	
	//-----------------------------//
	
	
	//-----------------------//
	// Calculation Functions //
	//-----------------------//
	
	//-----------------//
}
