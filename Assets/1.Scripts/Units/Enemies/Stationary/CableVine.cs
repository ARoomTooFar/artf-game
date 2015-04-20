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
	private float maxApproachRadius;
//	CableMaw MyMum;

	protected override void Awake () {
		base.Awake();
	}

	protected override void Start() {
		base.Start ();
		stun = new Stun ();
		constrict = new GenericDoT (1);
		maxApproachRadius = GetComponentInChildren<SphereCollider> ().radius;
	}

	protected override void initStates() {
		base.initStates ();

		State outranged = new State ("outranged");
		sM.states.Add (outranged.id, outranged);

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

	protected void redeploy () {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		float wait = 1.5f;

		do {
			/*
			this.facing.x =  Random.value * (this.facing.x == 0 ? (Random.value - 0.5f) : Mathf.Sign);
			this.facing.z = Random.value * (this.facing.z == 0 ? (Random.value - 0.5f) : Mathf.Sign);*/

			this.facing.x = 1 - Random.value * 2;
			this.facing.z = 1 - Random.value * 2;
			
			this.facing.Normalize();
		} while (this.facing == Vector3.zero);

	}

	protected bool outRanged() {
		if (this.target != null) {
			float distance = this.distanceToPlayer(this.target);
			return distance > this.maxApproachRadius && this.isHit;
		}
		return false;
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
