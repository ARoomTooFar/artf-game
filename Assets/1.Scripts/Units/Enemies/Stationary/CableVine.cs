using UnityEngine;
using System.Collections;

public class CableVine : StationaryEnemy {

	public Stun stun;
	public GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
//	CableMaw MyMum;

	// Use this for initialization
	protected void Awake () {
		base.Awake();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
