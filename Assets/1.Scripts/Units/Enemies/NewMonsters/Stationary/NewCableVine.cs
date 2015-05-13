using UnityEngine;
using System.Collections;

public class NewCableVine : NewStationaryEnemy {

	protected GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
	protected Transform origin;
	public float pull_velocity = 0.1f;
	private float maxApproachRadius;
	protected Blink blink;

	protected override void Awake () {
		base.Awake();
	}
	
	protected override void Start() {
		base.Start ();
		this.blink = this.inventory.items[inventory.selected].GetComponent<Blink>();
		constrict = new GenericDoT (1);
		maxApproachRadius = GetComponentInChildren<SphereCollider> ().radius;
		foreach(CVineDeploy behaviour in this.animator.GetBehaviours<CVineDeploy>()) {
			behaviour.blink = this.blink;
		}
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

	protected void redeploy() {
		if (isHit)
			Debug.Log ("rara");
		if ((this.target == null && this.isHit) || (this.target != null && (this.distanceToPlayer(this.target) >= maxApproachRadius) && this.isHit)) {
			this.animator.SetTrigger("redeployed");
		}
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
