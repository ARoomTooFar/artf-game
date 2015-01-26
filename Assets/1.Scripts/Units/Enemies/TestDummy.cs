using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TestDummy : Enemy {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		// Cause we have no animations for them atm
		if (animator)
			base.Update ();
	}
}
