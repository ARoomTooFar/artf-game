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
	private Stun debuff;
	private Collider col;
	private ParticleSystem particle;
	
	// public delegate 

	// Use this for initialization
	protected override void Start () {
		base.Start();

		this.col = this.GetComponent<Collider>();
		this.col.isTrigger = true;
		this.col.enabled = false;
		
		this.particle = this.GetComponent<ParticleSystem>();
		
		debuff = new Stun();
	}
	
	protected override void setInitValues() {
		base.setInitValues();

		cooldown = 5.0f;
		chgDist = 1;
		maxChgTime = 3.0f;
		hitWall = false;
		//print(this.opposition);
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
		
		this.col.enabled = true;
		user.animationLock = true;
		StartCoroutine(chargeFunc((chgDist + curChgTime) * 0.1f));
	}

	protected override void animDone() {
		user.animationLock = false;
		this.col.enabled = false;
		hitWall = false;
		enemies.Clear();

		base.animDone ();
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private IEnumerator chargeFunc(float chgTime) {
		this.particle.enableEmission = true;
		yield return StartCoroutine(chgTimeFunc(chgTime));
		this.particle.enableEmission = false;
		float tempStun = stunDuration * (hitWall ? 2 : 1);
		user.rb.velocity = Vector3.zero;
		foreach(Character ene in enemies) {
			// ((IStunable)ene.GetComponent(typeof(IStunable))).stun();

			ene.BDS.addBuffDebuff(debuff, this.gameObject, tempStun);
			ene.rb.velocity = Vector3.zero;
		}
		yield return StartCoroutine(chgLagTime());

		animDone();
	}


	
	// Timer and velocity changing thing
	private IEnumerator chgTimeFunc(float chgTime) {
		for (float timer = 0; timer <= chgTime; timer += Time.deltaTime) {

			if (hitWall) {
				user.GetComponent<Rigidbody>().velocity = Vector3.zero;
				yield break;
			}
			
			foreach(Character ene in enemies) {
				ene.transform.position = new Vector3(transform.position.x, ene.transform.position.y, transform.position.z);
				ene.BDS.addBuffDebuff(debuff, this.gameObject, 0.1f);
			}

			user.rb.velocity = user.facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
			yield return 0;
		}
	}
	
	private IEnumerator chgLagTime() {
		for (float timer = 0; timer < chgLag; timer += Time.deltaTime) {
			user.rb.velocity = Vector3.zero;
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
		IForcible<Vector3, float> component = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>));
		if(component != null && enemy != null) {
			enemies.Add (enemy);
		}
	}
}
