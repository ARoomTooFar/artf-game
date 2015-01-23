// Script for ToggleItems
//     Items that can be turned on to have an effect (Either indefinitly, for a duration, or when turned off)
//     Items with a duration turn off automatically and will have a cooldown based on how long it was active
// I Lied

using UnityEngine;
using System.Collections;

public class ToggleItem : Item {

	// In the base toggle class for now
	//     If we have indefinite skills, this may change
	protected int maxDuration;
	public float curDuration;

	public bool isActive;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		
		cooldown = 10.0f;
		maxDuration = 10;
		isActive = false;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
	
	// Called when character with an this item selected uses their item key
	// Atm, the child classes don't really use this, remove ovveride from future as needed
	public override void useItem() {
		if (isActive) {
			deactivateItem();
		} else {
			StartCoroutine(bgnEffect());
		}
	}

	protected virtual IEnumerator bgnEffect() {
		isActive = true;
		curDuration = maxDuration;
		while (curDuration > 0) {
			curDuration -= Time.deltaTime;
			if (curDuration > 0) {
				yield return null;
			}
		}
		deactivateItem();
	}
	
	public override void deactivateItem() {
		if (isActive) {
			atvDeactivation();
		}
		base.deactivateItem();
	}

	// If item is active when deactivated, these are called depending on the item
	protected virtual void atvDeactivation() {
		isActive = false;
		curCoolDown = cooldown - curDuration/2; // Adjust in someway based on ability in the future if needed
		curDuration = 0;

		animDone();
	}

	protected override void animDone() {
		base.animDone ();
	}
}
