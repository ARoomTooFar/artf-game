// Roll Item
// http://unitypatterns.com/scripting-with-coroutines/

using UnityEngine;
using System.Collections;

public class BullyTrunkBlast : QuickItem {

	public GameObject shockwave;
	
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
		GameObject wave = (GameObject)Instantiate(shockwave, user.transform.position, user.transform.rotation);
		wave.GetComponent<BullyTrunkShockwave>().setInitValues(user, opposition, 0, false, null);

		wave = (GameObject)Instantiate(shockwave, user.transform.position, user.transform.rotation);
		wave.GetComponent<BullyTrunkShockwave>().setInitValues(user, opposition, 0, true, null);
	}
}
