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

	protected override void setInitValues() {
		base.setInitValues ();
	}
	
	protected override void Update () {
		base.Update ();
	}

	protected override void Attack ()
	{
//		base.Attack ();
		target.GetComponent<Player> ().BDS.addBuffDebuff (constrict, this.gameObject, 4.0f);
		target.GetComponent<Player> ().BDS.addBuffDebuff (stun, this.gameObject, 4.0f);
	}
	
}
