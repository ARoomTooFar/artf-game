// Charge Item
// Issues with current implementation
//     - Checking for walls to stop uses current collider
//     - If monster is too big for collder to catch it may bug out
//     - Affects all character, will need someway to differentiate depending on user
//           * Making another charge specifically for enemies is advised against


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BullCharge : ChargeItem {
	
	public int chgDist;
	[Range(1, 4)]
	public int chargeSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;

	public List<Character> enemies;
	
	private bool hitWall;
	

	//private Collider collider;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		//collider = GetComponent<Collider>();
		collider.enabled = false;
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 5.0f;
		chgDist = 1;
		maxChgTime = 3.0f;
		hitWall = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		// user.animator.SetTrigger("Charging Charge"); Once we have the animation for it
	}

	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void chgDone() {
		// user.animator.SetTrigger("Charge Forward");
		
		collider.enabled = true;
		user.freeAnim = false;
		StartCoroutine(chargeFunc((chgDist + curChgTime) * 0.1f));
	}

	protected override void animDone() {
		user.freeAnim = true;
		collider.enabled = false;
		hitWall = false;
		enemies.Clear();

		base.animDone ();
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private IEnumerator chargeFunc(float chgTime) {
		yield return StartCoroutine(chgTimeFunc(chgTime));
		float tempStun = stunDuration * (hitWall ? 2 : 1);
		foreach(Character ene in enemies) {
			((IStunable)ene.GetComponent(typeof(IStunable))).stun();
		}
		yield return StartCoroutine(chgLagTime());

		animDone();
	}


	
	// Timer and velocity changing thing
	private IEnumerator chgTimeFunc(float chgTime) {
		for (float timer = 0; timer <= chgTime; timer += Time.deltaTime) {

			if (hitWall) {
				user.rigidbody.velocity = Vector3.zero;
				yield break;
			}
			
			foreach(Character ene in enemies) {
				ene.transform.position = transform.position;
				((IForcible<float>)ene.GetComponent(typeof(IForcible<float>))).push(0.1f);
			}

			user.rigidbody.velocity = user.facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
			yield return 0;
		}
	}
	
	private IEnumerator chgLagTime() {
		for (float timer = 0; timer < chgLag; timer += Time.deltaTime) {
			user.rigidbody.velocity = Vector3.zero;
			yield return 0;
		}
	}

	void OnTriggerEnter (Collider other) {
		RiotShield rShield = other.GetComponent<RiotShield>();
		if (other.tag == "Wall" || rShield && rShield.user.facing.normalized + user.facing.normalized == Vector3.zero) {
			hitWall = true;
		}

		// Will need a differentiation in the future(Or not if we want this)
		//     I suggest having the users know what is there enemy and settign ti that way somehow
		Character enemy = other.GetComponent<Character>();
		IForcible<float> component = (IForcible<float>) other.GetComponent( typeof(IForcible<float>));
		if(component != null && enemy != null) {
			enemies.Add (enemy);
		}
	}
}
