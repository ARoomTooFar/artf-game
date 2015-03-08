using UnityEngine;
using System.Collections;

public class Blink : QuickItem {
	
	[Range(1, 9)]
	public int blinkDistance;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
		cooldown = 10.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		// player.animator.SetTrigger("Blink"); Once we have the animation for it

		blinkFunc ();
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private void blinkFunc() {
		RaycastHit hit;
		float newDistance = blinkDistance;
		Vector3 newPosition;

		// Check for obstacles in out way
		if (user.GetComponent<Rigidbody>().SweepTest (user.facing, out hit, blinkDistance)) {
			if (hit.transform.tag == "Wall") {
				newDistance = hit.distance;
			}
		}

		newPosition = user.transform.position + user.facing.normalized * newDistance;

		while (!Physics.Linecast (newPosition, newPosition + Vector3.down * 5)) {
			newPosition = newPosition - user.facing.normalized * 0.1f;
		}

		user.GetComponent<Rigidbody>().MovePosition(newPosition);

		animDone();
	}
}
