// Roll Item
// http://unitypatterns.com/scripting-with-coroutines/

using UnityEngine;
using System.Collections;

public class BullyTrunkShockwave : QuickItem {

	public GameObject leftShockwave, rightShockwave;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		// user.animator.SetTrigger("Roll"); Once we have the animation for it
		
		this.unleashTheBeast ();
		this.animDone ();
	}
	
	protected override void animDone() {
		base.animDone();
	}

	private void unleashTheBeast() {
	}
}
