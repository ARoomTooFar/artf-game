// Blinks onto the other side of the target

using UnityEngine;
using System.Collections;

public class BlinkStrike : QuickItem {

	public NewMirage mirage;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
		cooldown = 0.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();

		blinkFunc ();
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	protected virtual void blinkFunc() {
		Vector3 targetPos = this.mirage.deathTarget.transform.position;
		
		Vector3 newPosition = targetPos + user.facing.normalized;
		newPosition.y = this.mirage.transform.position.y;

		// user.transform.position = newPosition;
		user.GetComponent<Rigidbody>().MovePosition(newPosition);
		
		animDone();
	}
}
