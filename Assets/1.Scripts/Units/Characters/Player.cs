// Character class
// Base class that our heroes will inherit from

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
}