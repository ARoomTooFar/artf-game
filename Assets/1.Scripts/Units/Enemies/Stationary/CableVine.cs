using UnityEngine;
using System.Collections;

public class CableVine : StationaryEnemy {

	public Stun stunObj;
	public GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
//	CableMaw MyMum;

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
