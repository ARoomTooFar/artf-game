using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForcePush : ChargeItem {
	public int chgDist;
	[Range(1, 4)]
	public int chargeSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;
	private bool hit;
	private Vector3 facing;
	public List<Character> foes;
	//private Collider collider;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		//collider = GetComponent<Collider>();
		collider.enabled = false;
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 5.0f;
		chgDist = 1;
		maxChgTime = 3.0f;
		hit = false;
	}
	public override void useItem() {
		base.useItem ();
		// user.animator.SetTrigger("Charging Charge"); Once we have the animation for it
	}

	public override void deactivateItem() {
		base.deactivateItem();
	}
	// Update is called once per frame
	protected override void chgDone() {
		// user.animator.SetTrigger("Charge Forward");
		
		//collider.enabled = true;
		user.freeAnim = false;
		facing = user.facing;
		StartCoroutine(chargeFunc((chgDist + curChgTime) * 0.1f));
	}

	protected override void animDone() {
		user.freeAnim = true;
		collider.enabled = false;
		hit = false;
	    rigidbody.isKinematic = true;
		foes.Clear();
		base.animDone ();
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private IEnumerator chargeFunc(float chgTime) {
		rigidbody.isKinematic = false;
		collider.enabled = true;
		yield return StartCoroutine(chgTimeFunc(chgTime));
		//float tempStun = stunDuration * (hitWall ? 2 : 1);
		
		yield return StartCoroutine(retTimeFunc(chgTime));
		yield return StartCoroutine(chgLagTime());

		animDone();
	}


	
	// Timer and velocity changing thing
	private IEnumerator chgTimeFunc(float chgTime) {
		for (float timer = 0; timer <= chgTime; timer += Time.deltaTime) {
			rigidbody.velocity = facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
			transform.localScale += new Vector3(0.20f,0,0.20f);
			foreach(Character foe in foes) {
				foe.rigidbody.velocity = facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
			}
			//((IForcible<float>)foe.GetComponent(typeof(IForcible<float>))).push(0.1f);
			yield return 0;
		}
	}
	private IEnumerator retTimeFunc(float chgTime) {
		for (float timer = 0; timer <= chgTime; timer += Time.deltaTime) {
            transform.localScale -= new Vector3(0.20f,0,0.20f);
			rigidbody.velocity = -facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
			/*if (!hit) {
				rigidbody.velocity = Vector3.zero;
				yield break;
			}*/
			foreach(Character foe in foes) {
				foe.rigidbody.velocity = facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
				//((IForcible<float>)foe.GetComponent(typeof(IForcible<float>))).push(0.1f);
			}
			yield return 0;
		}
	}
	
	private IEnumerator chgLagTime() {
		for (float timer = 0; timer < chgLag; timer += Time.deltaTime) {
			rigidbody.velocity = Vector3.zero;
			yield return 0;
		}
	}

	void OnTriggerEnter (Collider other) {
			RiotShield rShield = other.GetComponent<RiotShield>();
			if (other.tag == "Wall" || rShield && rShield.user.facing.normalized + user.facing.normalized == Vector3.zero) {
				hit = true;
			}
			IForcible<float> component = (IForcible<float>) other.GetComponent( typeof(IForcible<float>) );
			Character foe = other.GetComponent<Character>();
			if( component != null && foe != null) {
				foes.Add (foe);
			}
		// Will need a differentiation in the future(Or not if we want this)
		//     I suggest having the users know what is there enemy and settign ti that way somehow

	}
}
