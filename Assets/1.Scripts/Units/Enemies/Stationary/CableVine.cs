using UnityEngine;
using System.Collections;

public class CableVine : StationaryEnemy {
	
	protected Stun stun;
	protected GenericDoT constrict;
	bool inStealth;
	HingeJoint tether;
	Rigidbody joints;
	protected Transform origin;
	protected float pull_velocity = 12;
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
		/*
		if(this.isApproaching()) {
			pulltarget = target.GetComponent<Player>();
		}

		if (pulltarget != null)
			Debug.Log (pulltarget.rb.velocity.magnitude);*/

		base.Update ();
	}

	protected override void Approach() {
		base.Approach ();
		target.transform.position -= this.facing * 0.001f;
/*		if (pulltarget == null){
			Debug.Log ("target is null");
		}
		else {
//			Debug.Log(pulltarget.rb.velocity);
			pulltarget.rb.velocity += pullVector ();
		//	Debug.Log (pulltarget.rb.velocity.magnitude);
		}*/
	}

	protected override void Attack ()
	{
//		base.Attack ();
		target.GetComponent<Player> ().BDS.addBuffDebuff (constrict, this.gameObject, 4.0f);
		target.GetComponent<Player> ().BDS.addBuffDebuff (stun, this.gameObject, 4.0f);
	}

	/*
	private Vector3 pullVector(){
		return pulltarget.rb.velocity.normalized * pull_velocity * -1.0f;
	}*/
	
}
