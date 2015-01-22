// Riot Shield item

using UnityEngine;
using System.Collections;

public class RiotShield : ToggleItem {
	private MeshRenderer meshRenderer;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		meshRenderer = GetComponent<MeshRenderer>();
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 10.0f;
		maxDuration = 5;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem();
		
		collider.enabled = true;
		meshRenderer.enabled = true;
		
		// player.animator.SetTrigger("Sprint"); Set speed var in animator once we have the animation
	}
	
	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void atvDeactivation() {
		collider.enabled = false;
		meshRenderer.enabled = false;


		base.atvDeactivation();
	}
}
