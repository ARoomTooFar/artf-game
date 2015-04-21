// Enemies that can move

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewMobileEnemy : NewEnemy {

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
		this.rb.isKinematic = true;
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
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 2.0f;
	}
	
	//----------------------//
	
	//----------------------//
	// Transition Functions //
	//----------------------//

	
	//---------------------//
	
	
	//------------------//
	// Action Functions //
	//------------------//
	
	public virtual void StopSearch() {
		this.searchController.Stop ();
		this.searchStateController.Stop ();
	}

	
	//------------------//
	
	
	
	//-----------------------------//
	// Coroutines for timing stuff //
	//-----------------------------//
	
	protected IEnumerator moveToPosition(Vector3 position) {
		while (Vector3.Distance(this.transform.position, this.targetDir) > 0.25f&& this.target == null) {
			this.rb.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			yield return null;
		}
	}
	
	protected IEnumerator moveToExpectedArea() {
		this.facing = this.targetDir;
		float moveToTime = 0.5f;
		while (Vector3.Distance(this.transform.position, this.targetDir) > 0.1f && this.target == null && moveToTime > 0.0f) {
			this.rb.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
			moveToTime -= Time.deltaTime;
			yield return null;
		}
	}
	
	protected IEnumerator randomSearch() {;
		float resetTimer = aggroTimer;
		while(resetTimer > 0.0f && this.target == null) {
			this.facing = new Vector3(Random.Range (-1.0f, 1.0f), 0.0f, Random.Range (-1.0f, 1.0f)).normalized;
			this.rb.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
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
