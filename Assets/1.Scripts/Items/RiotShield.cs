// Riot Shield item

using UnityEngine;
using System.Collections;

public class RiotShield : Item {
	
	private int maxDuration = 10;
	public float curDuration;

	private bool isActive;
	private MeshRenderer meshRenderer;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		meshRenderer = GetComponent<MeshRenderer>();
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
		
		collider.enabled = true;
		meshRenderer.enabled = true;
		
		isActive = true;
		curDuration = maxDuration;
		
		// player.animator.SetTrigger("Sprint"); Set speed var in animator once we have the animation
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
		
		if (isActive) {
			collider.enabled = false;
			meshRenderer.enabled = false;
			isActive = false;
			curCoolDown = cooldown - curDuration/2;
			curDuration = 0;
		}
	}
}
