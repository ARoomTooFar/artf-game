// Sprint item

using UnityEngine;
using System.Collections;

public class Sprint : ToggleItem {

	[Range(2, 4)]
	public int sprintSpeed;
	private int baseSpeed;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 15.0f;
		maxDuration = 10;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if (curDuration > 0) {
			curDuration -= Time.deltaTime;
			if (curDuration <= 0) {
				deactivateItem();
			}
		}
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem();


		baseSpeed = player.stats.speed;
		player.stats.speed *= sprintSpeed;


		// player.animator.SetTrigger("Sprint"); Set speed var in animator once we have the animation
	}

	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void atvDeactivation() {
		player.stats.speed = baseSpeed;

		base.atvDeactivation();
	}
}
