// Script for ChargeItems
//     Items that can charged to be stronger
//     Cooldown increased based on how long it is charged

using UnityEngine;
using System.Collections;

public class ChargeItem : Item {

	public float curChgTime;
	protected float _maxChgTime;
	public float maxChgTime {
		get {return _maxChgTime;}
		protected set {_maxChgTime = value;}
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		itemType = 'C';
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
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem();
		if (cdBar != null) {
			cdBar.onState = 2;
			cdBar.max = 3;
		}
		StartCoroutine(bgnCharge());
	}

	// If things need to be done while charging make this virtual 
	protected virtual IEnumerator bgnCharge() {
		curChgTime = 0.0f;
		while (user.inventory.keepItemActive) {
			curChgTime = Mathf.Clamp(curChgTime + Time.deltaTime, 0.0f, maxChgTime);
			if (cdBar != null) cdBar.current = curChgTime + Time.deltaTime;
			yield return null;
		}
		if (cdBar != null) cdBar.current = curChgTime;
		deactivateItem();
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
		if (curChgTime >= 0.0f) {
			chgDone();
		}
	}

	protected override void animDone() {
		curCoolDown = cooldown + (curChgTime * 3);
		if (cdBar != null) {
			cdBar.onState = 1;
			cdBar.max = curCoolDown;
		}
		curChgTime = -1.0f;

		base.animDone ();
	}

	// Once key to charge is released (Or other stuff like taking damage), do what are ability is
	protected virtual void chgDone() {
	}
}
