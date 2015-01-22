// Script for QuickItems
//     Items that activate  and go on cooldown once button is pressed

using UnityEngine;
using System.Collections;

public class QuickItem : Item {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		cooldown = 10.0f;
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
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
	}
}
