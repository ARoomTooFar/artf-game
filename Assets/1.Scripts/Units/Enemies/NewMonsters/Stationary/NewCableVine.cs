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
		constrict = new GenericDoT (1);
		maxApproachRadius = GetComponentInChildren<SphereCollider> ().radius;
		foreach(CVineDeploy behaviour in this.animator.GetBehaviours<CVineDeploy>()) {
			behaviour.blink = this.blink;
		}

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

	protected virtual void isApproaching() {
		// If we don't have a target currently and aren't alerted, automatically assign anyone in range that we can see as our target
		if (this.target == null) {
			if (aRange.unitsInRange.Count > 0) {
				foreach(Character tars in aRange.unitsInRange) {
					if (this.canSeePlayer(tars.gameObject)) {
						animator.SetBool("Alerted", true);
						target = tars.gameObject;
						break;
					}
				}
				
				if (target == null) {
					animator.SetBool("InAttackRange", false);
					return;
				}
			} else {
				animator.SetBool("InAttackRange", false);
				return ;
			}
		}
		
		float distance = this.distanceToPlayer(this.target);
		
		if (distance >= this.maxApproachRadius && this.canSeePlayer (this.target)) {
			// agent.alerted = true;
			animator.SetBool("InAttackRange", true);
		}
	}

	protected void redeploy() {
		if (this.lastSeenPosition != null) {
			float distance = this.distanceToPlayer(this.target);
			if(this.isHit) { Debug.Log ("ouch"); Debug.Log (distance); Debug.Log (maxApproachRadius);}
			this.animator.SetBool("redeployed", distance > this.maxApproachRadius && this.isHit);
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
	
	public override void damage(int dmgTaken, Character striker) {
		base.damage (dmgTaken, striker);
		target = striker.gameObject;
	}
	
}
