using UnityEngine;
using System.Collections;

public class CableVine : StationaryEnemy {
	
	protected Stun stun;
	protected GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
	protected Transform origin;
	protected float pull_velocity = 0.1f;
	Player pulltarget;
//	CableMaw MyMum;

	protected override void Awake () {
		base.Awake();
	}

	protected override void Start() {
		base.Start ();
		stun = new Stun ();
		constrict = new GenericDoT (1);
	}

	protected override void setInitValues() {
		base.setInitValues ();
	}
	
	protected override void Update () {
		base.Update ();
	}

	protected override void Approach() {
		base.Approach ();
		target.transform.position = target.transform.position - pullVelocity();
		target.GetComponent<Player> ().BDS.addBuffDebuff (constrict, this.gameObject);
	}

	protected override void Attack ()
	{
//		base.Attack ();
		target.GetComponent<Player> ().BDS.addBuffDebuff (constrict, this.gameObject);
		target.GetComponent<Player> ().BDS.addBuffDebuff (stun, this.gameObject, 4.0f);
	}

	private Vector3 pullVelocity(){
		float time = this.facing.magnitude/pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = this.facing.x / time;
		velocity.y = this.facing.y / time;
		velocity.z = this.facing.z / time;
		return velocity;
	}

}
