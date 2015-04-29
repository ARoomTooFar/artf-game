using UnityEngine;
using System.Collections;

public class NewCableVine : NewStationaryEnemy {
	
	//protected Stun stunDebuff;
	protected GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
	protected Transform origin;
	public float pull_velocity = 0.1f;
	private float maxApproachRadius;
	protected Blink blink;

	//	CableMaw MyMum;
	
	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		//stunDebuff = new Stun ();
		this.blink = this.inventory.items[inventory.selected].GetComponent<Blink>();
		constrict = new GenericDoT (1);
		maxApproachRadius = GetComponentInChildren<SphereCollider> ().radius;
		foreach(CVineDeploy behaviour in this.animator.GetBehaviours<CVineDeploy>()) {
			behaviour.blink = this.blink;
		}

//		this.blink = this.inventory.items[inventory.selected].GetComponent<Blink> ();
//		if (this.blink == null) Debug.LogWarning ("Cable Vine does not have Blink equipped");
	}

	
	protected override void setInitValues() {
		base.setInitValues ();
	}
	
	protected override void Update () {
		base.Update ();
		redeploy ();
		if (isHit && target != null)
			isHit = false;
	}

	/*
	protected void redeploy () {
		this.facing = this.target.transform.position - this.transform.position;
		this.facing.y = 0.0f;
		float wait = 1.5f;
		
		if (this.blink.curCoolDown <= 0) {
			do {

				this.facing.x =  Random.value * (this.facing.x == 0 ? (Random.value - 0.5f) : Mathf.Sign);
				this.facing.z = Random.value * (this.facing.z == 0 ? (Random.value - 0.5f) : Mathf.Sign);
				
				this.facing.x += Mathf.Sign (this.facing.x) * Random.value * 10;
				this.facing.z += Mathf.Sign (this.facing.z) * Random.value * 10;
				
				this.facing.Normalize ();
			} while (this.facing == Vector3.zero);
			
			this.blink.useItem ();
		}
		
	}*/
	

	protected void redeploy() {
		if (isHit)
			Debug.Log ("rara");
		if ((this.target == null && this.isHit) || (this.target != null && (this.distanceToPlayer(this.target) >= maxApproachRadius) && this.isHit)) {
			this.animator.SetTrigger("redeployed");

		}
	}
	
	private Vector3 pullVelocity(){
		float time = this.facing.magnitude/pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = this.facing.x / time;
		velocity.y = this.facing.y / time;
		velocity.z = this.facing.z / time;
		return velocity;
	}


	// OVERRIDE

	public override void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		base.damage (dmgTaken, atkPosition, source);
		if (target == null)
			this.lastSeenPosition = atkPosition.position;
	}
	
	public override void damage(int dmgTaken, Transform atkPosition) {
		base.damage(dmgTaken, atkPosition);
		if (target == null && this.lastSeenPosition == null) this.lastSeenPosition = atkPosition.position;
	}

}
