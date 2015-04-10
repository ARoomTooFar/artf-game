using UnityEngine;
using System.Collections;

public class CableVine : StationaryEnemy {
	
	protected Stun stun;
	protected GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
	protected Transform origin;
//	CableMaw MyMum;

	protected override void Awake () {
		base.Awake();
	}

	protected override void Start() {
		base.Start ();
		stun = new Stun ();
		constrict = new GenericDoT (10);
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
