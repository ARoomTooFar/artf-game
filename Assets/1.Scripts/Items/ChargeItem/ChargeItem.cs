// Script for ChargeItems
//     Items that can charged to be stronger
//     Cooldown increased based on how long it is charged

using UnityEngine;
using System.Collections;

public class ChargeItem : Item {

	public float curChgTime;
	protected float maxChgTime;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 10.0f;
		curChgTime = -1.0f;
		maxChgTime = 2.0f;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (curChgTime >= 0.0f) {
			curChgTime = Mathf.Clamp(curChgTime + Time.deltaTime, 0.0f, maxChgTime);
		}
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem();
		curChgTime = 0.0f;
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
		if (curChgTime >= 0.0f) {
			chgDone();
		}
	}

	// Once key to charge is released (Or other stuff like taking damage), do what are ability is
	protected virtual void chgDone() {
	}
}
