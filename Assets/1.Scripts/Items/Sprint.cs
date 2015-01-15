// Sprint item

using UnityEngine;
using System.Collections;

public class Sprint : Item {
	
	private int maxDuration = 10;
	public float curDuration;

	[Range(2, 4)]
	public int sprintSpeed;

	private int baseSpeed;
	private bool isActive;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	protected override void setInitValues() {
		cooldown = 15.0f;
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
		// base.useItem();

		isActive = true;
		baseSpeed = player.stats.speed;
		player.stats.speed *= sprintSpeed;
		curDuration = maxDuration;

		// player.animator.SetTrigger("Sprint"); Set speed var in animator once we have the animation
	}

	public override void deactivateItem() {
		base.deactivateItem();

		if (isActive) {
			isActive = false;
			player.stats.speed = baseSpeed;
			curCoolDown = cooldown - curDuration/2;
			curDuration = 0;
		}
	}
}
