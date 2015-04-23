// Lunge item
//     If any enemies are in range, it will jump towards the closest one

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lunge : QuickItem {
	protected Collider col;
	protected List<Character> enemiesInRange;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		this.col = this.GetComponent<Collider>();
		this.col.enabled = true;

		this.enemiesInRange = new List<Character>();
	}
	
	protected override void setInitValues() {
		cooldown = 10.0f;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	public override void useItem() {
		Character closestEnemy = null;
		float dis;
		float distance = float.MaxValue;
		for (int count = 0; count < enemiesInRange.Count; count++) {
			if (!this.user.CanSeeObject(enemiesInRange[count].gameObject)) {
				enemiesInRange.RemoveAt(count);
				count--;
			} else {
				dis = Vector3.Distance(this.user.transform.position, enemiesInRange[count].transform.position);
				if (dis >= distance) continue;

				distance = dis;
				closestEnemy = enemiesInRange[count];
			}
		}
		if (this.enemiesInRange.Count == 0) return;

		curCoolDown = cooldown;
		user.stunned = true;
		this.StartCoroutine(LungeFunction(closestEnemy));
	}
	
	protected override void animDone() {
		user.stunned = false;
		base.animDone ();
	}

	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	protected IEnumerator LungeFunction(Character target) {
		yield return StartCoroutine(LungeTimeFunction(target));
		user.GetComponent<Rigidbody>().velocity = Vector3.zero;
		animDone();
	}
	
	// Timer and velocity changing thing
	protected IEnumerator LungeTimeFunction(Character target) {
		while (!this.user.col.bounds.Intersects(target.col.bounds)) {
			this.user.facing = this.user.GetFacingTowardsObject(target.gameObject);
			this.user.transform.localRotation = Quaternion.LookRotation(this.user.facing);	
			this.user.rb.velocity = user.facing.normalized * 60f;
			yield return 0;
		}
	}

	// Tracks all oppositions
	void OnTriggerEnter (Collider other) {
		Character enemy = (Character) other.GetComponent(this.opposition);
		if(enemy != null) {
			enemiesInRange.Add (enemy);
		}
	}

	void OnTriggerExit(Collider other) {
		Character enemy = (Character) other.GetComponent(this.opposition);
		if(enemy != null) {
			enemiesInRange.Remove (enemy);
		}
	}
}
