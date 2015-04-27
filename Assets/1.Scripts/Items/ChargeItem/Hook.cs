using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hook : ChargeItem {
	public int chgDist;
	[Range(1, 4)]
	public int chargeSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;
	private bool hit;
	private bool check;
	private Vector3 facing;
	private Character foe;
	private Stun debuff;
	private Immobilize immobil;

	private Collider col;
	private Renderer ren;
	private Rigidbody rb;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		this.col = this.GetComponent<Collider>();
		this.col.enabled = false;

		this.ren = this.GetComponent<Renderer>();
		this.rb = this.GetComponent<Rigidbody>();

		debuff = new Stun();
		immobil = new Immobilize();
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

		this.ren.enabled = true;
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
		user.BDS.rmvBuffDebuff(immobil,this.gameObject);
		user.freeAnim = true;
		this.col.enabled = false;
		hit = false;
		this.ren.enabled = false;
	    this.rb.isKinematic = true;
		foe = null;
		
		base.animDone ();
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private IEnumerator chargeFunc(float chgTime) {
		this.rb.isKinematic = false;
		this.col.enabled = true;
		yield return StartCoroutine("chgTimeFunc",(chgTime));
		yield return StartCoroutine("chgLagTime");
		animDone();
	}
	
	// Timer and velocity changing thing
	private IEnumerator chgTimeFunc(float chgTime) {
		float totalTime = chgTime*2;
		float checkTime = 0;
		user.BDS.addBuffDebuff(debuff, this.gameObject, totalTime);
		for (float timer = 0; timer <= totalTime; timer += Time.deltaTime) {
			if(timer <= totalTime/2){
				GetComponent<Rigidbody>().velocity = facing.normalized * user.stats.speed * 1.5f * chargeSpeed;
				if(foe!=null){
					foe.BDS.addBuffDebuff(debuff, this.gameObject, stunDuration);
					foe.transform.position = transform.position;
				}
			}else if(timer > totalTime/2){
				if(foe!=null){
					foe.BDS.addBuffDebuff(debuff, this.gameObject, stunDuration);
					foe.transform.position = transform.position;
				}
				GetComponent<Rigidbody>().velocity = -facing.normalized * user.stats.speed * 1.5f * chargeSpeed;	
			}
			if(check){
			   checkTime = (totalTime/2) - timer;
			   timer = totalTime/2;
			   timer += checkTime;
			   check = false;
			   checkTime = 0;
			}
			//((IForcible<float>)foe.GetComponent(typeof(IForcible<float>))).push(0.1f);
			yield return 0;
		}
	}
	
	private IEnumerator chgLagTime() {
		for (float timer = 0; timer < chgLag; timer += Time.deltaTime) {
			this.rb.velocity = Vector3.zero;
			yield return 0;
		}
	}

	void OnTriggerEnter (Collider other) {
		if(!hit){
			if (other.tag == "Wall") {
				hit = true;
				check = true;
			}
			IForcible<Vector3,float> component = (IForcible<Vector3,float>) other.GetComponent( typeof(IForcible<Vector3,float>) );
			foe = other.GetComponent<Character>();
			if( component != null && foe != null) {
				hit = true;
				check = true;
				this.col.enabled = false;
			}
		}
	}
}
